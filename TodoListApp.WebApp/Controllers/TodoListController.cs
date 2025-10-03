using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Common.Parameters.Filtering;
using TodoListApp.Common.Parameters.Sorting;
using TodoListApp.WebApp.Areas.Identity.Data;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;
using TodoListApp.WebApp.Utility;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class TodoListController : BaseController
{
    private readonly ITodoListWebApiService listService;
    private readonly ITodoTaskWebApiService taskService;

    public TodoListController(ITodoListWebApiService service, ITodoTaskWebApiService taskService, UserManager<ApplicationUser> userManager)
        : base(userManager)
    {
        this.listService = service;
        this.taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    {
        var userId = await this.GetCurrentUserId();

        var apiResponse = await this.listService.GetPagedTodoListsAsync(userId, pageNumber, pageSize);

        var viewModel = new TodoListIndexViewModel
        {
            TodoLists = apiResponse.Data.Select(x => WebAppMapper.MapTodoList<TodoList, TodoListViewModel>(x)).ToList(),
            CurrentPage = apiResponse.Pagination?.CurrentPage ?? 1,
            TotalPages = apiResponse.Pagination?.TotalPages ?? 1,
            PageSize = apiResponse.Pagination?.PageSize ?? 10,
            TotalCount = apiResponse.Pagination?.TotalCount ?? 0,
            HasPrevious = apiResponse.Pagination?.HasPrevious ?? false,
            HasNext = apiResponse.Pagination?.HasNext ?? false,
        };

        return this.View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(
        int listId,
        int pageNumber = 1,
        int pageSize = 5,
        TodoTaskStatusFilterOption statusFilterOption = TodoTaskStatusFilterOption.All,
        TaskSortingOptions sorting = TaskSortingOptions.CreatedDateDesc)
    {
        var userId = await this.GetCurrentUserId();

        var todo = await this.listService.GetTodoListByIdAsync(listId, userId);

        if (todo == null)
        {
            return this.NotFound();
        }

        var tasks = await this.taskService.GetPagedTodoTasksByListAsync(listId, pageNumber, pageSize, statusFilterOption, sorting);

        var result = WebAppMapper.MapTodoList<TodoList, TodoListViewModel>(todo);
        result.TodoTaskIndex = new TodoTaskIndexViewModel()
        {
            TodoTasks = tasks.Data.Select(x => WebAppMapper.MapTodoTask<TodoTask, TodoTaskViewModel>(x)).ToList() ?? new List<TodoTaskViewModel>(),
            CurrentPage = tasks.Pagination?.CurrentPage ?? 1,
            HasNext = tasks.Pagination?.HasNext ?? false,
            HasPrevious = tasks.Pagination?.HasPrevious ?? false,
            PageSize = tasks.Pagination?.PageSize ?? 10,
            TotalCount = tasks.Pagination?.TotalCount ?? 1,
            TotalPages = tasks.Pagination?.TotalPages ?? 1,
            Sorting = sorting,
            StatusFilterOption = statusFilterOption,
        };

        return this.View(result);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEdit(int id = 0)
    {
        if (id == 0)
        {
            return this.View(new TodoListViewModel());
        }

        var userId = await this.GetCurrentUserId();
        var todo = await this.listService.GetTodoListByIdAsync(id, userId);   // userId

        if (todo == null)
        {
            return this.View("Error", new ErrorViewModel { RequestId = $"List with id {id} could not be found.", ReturnUrl = new Uri($"/todolist/details?listId={id}", UriKind.Relative) });
        }

        var model = WebAppMapper.MapTodoList<TodoList, TodoListViewModel>(todo);

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEdit(TodoListViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            this.ModelState.AddModelError(" ", "Not Valid");
            return this.View("CreateEdit", model);
        }

        var userId = await this.GetCurrentUserId();

        try
        {
            TodoList todo = new TodoList
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                UserId = userId,
            };

            var act = model.Id switch
            {
                not 0 => this.listService.UpdateTodoListAsync(todo),
                _ => this.listService.CreateTodoListAsync(todo),
            };

            var result = await act;

            if (result == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.RedirectToAction(nameof(this.Details), new { listId = result.Id });
        }
        catch (ApplicationException ex)
        {
            this.ViewBag.ErrorMessage = ex.Message;
            return this.View("Error", new ErrorViewModel { RequestId = ex.Message, ReturnUrl = new Uri("/todolist", UriKind.Relative) });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = await this.GetCurrentUserId();

        var todo = await this.listService.GetTodoListByIdAsync(id, userId);

        if (todo == null)
        {
            this.ViewBag.ErrorMessage = "No lists found with this id.";
            return this.View("Error");
        }

        var result = await this.listService.DeleteTodoListAsync(id, userId);

        if (!result)
        {
            this.ViewBag.ErrorMessage = "Error when deleting todolist.";
            return this.View("Error", new ErrorViewModel { RequestId = "Internal Error when deleting list", ReturnUrl = new Uri("/todolist", UriKind.Relative) });
        }

        return this.RedirectToAction(nameof(this.Index));
    }
}
