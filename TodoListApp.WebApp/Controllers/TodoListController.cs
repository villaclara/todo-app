using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Controllers;

public class TodoListController : Controller
{
    private readonly ITodoListWebApiService listService;
    private readonly ITodoTaskWebApiService taskService;

    public TodoListController(ITodoListWebApiService service, ITodoTaskWebApiService taskService)
    {
        this.listService = service;
        this.taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
    {
        var apiResponse = await this.listService.GetPagedTodoListsAsync(1, pageNumber, pageSize);

        var viewModel = new TodoListIndexViewModel
        {
            TodoLists = apiResponse.Data.Select(x => new TodoListViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                UserId = x.UserId,
            }).ToList(),
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
    public async Task<IActionResult> Details(int listId, int pageNumber = 1, int pageSize = 5)
    {
        var todo = await this.listService.GetTodoListByIdAsync(listId, 1);

        if (todo == null)
        {
            return this.NotFound();
        }

        var tasks = await this.taskService.GetPagedTodoTasksAsync(listId, pageNumber, pageSize);

        var obj = new TodoListViewModel
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            UserId = todo.UserId,
            Tasks = tasks.Data.Select(x => new TodoTaskViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Assignee = x.Assignee,
                CreatedAtDate = x.CreatedAtDate,
                DueToDate = x.DueToDate,
                TaskStatus = x.TaskStatus,
                TodoListId = x.TodoListId,
            }).ToList() ?? new List<TodoTaskViewModel>(),
            TodoTaskIndex = new TodoTaskIndexViewModel()
            {
                TodoTasks = tasks.Data.Select(x => new TodoTaskViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Assignee = x.Assignee,
                    CreatedAtDate = x.CreatedAtDate,
                    DueToDate = x.DueToDate,
                    TaskStatus = x.TaskStatus,
                    TodoListId = x.TodoListId,
                }).ToList() ?? new List<TodoTaskViewModel>(),
                CurrentPage = tasks.Pagination?.CurrentPage ?? 1,
                HasNext = tasks.Pagination?.HasNext ?? false,
                HasPrevious = tasks.Pagination?.HasPrevious ?? false,
                PageSize = tasks.Pagination?.PageSize ?? 10,
                TotalCount = tasks.Pagination?.TotalCount ?? 1,
                TotalPages = tasks.Pagination?.TotalPages ?? 1,
            },
        };

        return this.View(obj);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEdit(int id = 0)
    {
        if (id == 0)
        {
            return this.View(new TodoListViewModel());
        }

        var todo = await this.listService.GetTodoListByIdAsync(id, 1);   // userId

        if (todo == null)
        {
            return this.View("Error", new ErrorViewModel { RequestId = $"List with id {id} could not be found.", ReturnUrl = new Uri($"/todolist/details?listId={id}", UriKind.Relative) });
        }

        var model = new TodoListViewModel
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            UserId = todo.UserId,
        };

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

        try
        {
            TodoList todo = new TodoList
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                UserId = 1, // TODO - Set user Id
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
        var todo = await this.listService.GetTodoListByIdAsync(id, 1);  // TODO - id

        if (todo == null)
        {
            this.ViewBag.ErrorMessage = "No lists found with this id.";
            return this.View("Error");
        }

        var result = await this.listService.DeleteTodoListAsync(id, 1); // TODO - id

        if (!result)
        {
            this.ViewBag.ErrorMessage = "Error when deleting todolist.";
            return this.View("Error", new ErrorViewModel { RequestId = "Internal Error when deleting list", ReturnUrl = new Uri("/todolist", UriKind.Relative) });
        }

        return this.RedirectToAction(nameof(this.Index));
    }
}
