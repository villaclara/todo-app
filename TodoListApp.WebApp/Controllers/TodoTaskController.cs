using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Common.Models.Enums;
using TodoListApp.Common.Models.TodoTaskCommentModels;
using TodoListApp.Common.Parameters.Filtering;
using TodoListApp.Common.Parameters.Pagination;
using TodoListApp.Common.Parameters.Sorting;
using TodoListApp.WebApp.Areas.Identity.Data;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;
using TodoListApp.WebApp.Utility;

namespace TodoListApp.WebApp.Controllers;

[Authorize]
public class TodoTaskController : BaseController
{
    private readonly ITodoTaskWebApiService taskService;
    private readonly ITodoTaskTagWebApiService tagService;
    private readonly ITodoTaskCommentWebApiService commentService;
    private readonly ITodoListWebApiService listService;
    private readonly UsersDbContext context;

    public TodoTaskController(
        ITodoTaskWebApiService taskService,
        ITodoTaskTagWebApiService tagService,
        ITodoTaskCommentWebApiService commentService,
        ITodoListWebApiService listService,
        UserManager<ApplicationUser> userManager,
        UsersDbContext context)
        : base(userManager)
    {
        this.taskService = taskService;
        this.tagService = tagService;
        this.commentService = commentService;
        this.listService = listService;
        this.context = context;
    }

    public async Task<IActionResult> Index(
        PaginationParameters pagination,
        TodoTaskAssigneeFilter filter,
        TodoTaskStatusFilterOption statusFilterOption = TodoTaskStatusFilterOption.NotCompleted,
        TaskSortingOptions sorting = TaskSortingOptions.DueDateAsc)
    {
        var userId = await this.GetCurrentUserId();

        var apiResponse = await this.taskService.GetPagedTodoTasksByAssigneeAsync(userId, pagination, filter, statusFilterOption, sorting);

        var viewModel = new TodoTaskIndexViewModel
        {
            TodoTasks = apiResponse.Data.Select(x => WebAppMapper.MapTodoTask<TodoTask, TodoTaskViewModel>(x)).ToList(),
            CurrentPage = apiResponse.Pagination?.CurrentPage ?? 1,
            TotalPages = apiResponse.Pagination?.TotalPages ?? 1,
            PageSize = apiResponse.Pagination?.PageSize ?? 10,
            TotalCount = apiResponse.Pagination?.TotalCount ?? 0,
            HasPrevious = apiResponse.Pagination?.HasPrevious ?? false,
            HasNext = apiResponse.Pagination?.HasNext ?? false,
            Sorting = sorting,
            StatusFilterOption = statusFilterOption,
            Filter = filter,
        };
        return this.View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, int listId, bool fromTodoList = false)
    {
        var apiResponse = await this.taskService.GetTodoTaskByIdAsync(id, listId);

        if (apiResponse == null)
        {
            return this.NotFound();
        }

        var viewModel = WebAppMapper.MapTodoTask<TodoTask, TodoTaskViewModel>(apiResponse);

        var comments = await this.commentService.GetCommentsForTaskAsync(id);

        if (comments == null)
        {
            return this.NotFound();
        }

        var userId = await this.GetCurrentUserId();

        var isCurrentUserTaskOwner = true;
        var list = await this.listService.GetTodoListByIdAsync(viewModel.TodoListId, userId);
        if (list == null)
        {
            isCurrentUserTaskOwner = false;
            viewModel.ReturnUrl = new Uri($"/todotask", UriKind.Relative);
        }
        else
        {
            viewModel.ReturnUrl = fromTodoList ? new Uri($"/todolist/details?listId={viewModel.TodoListId}", UriKind.Relative) : new Uri($"/todotask", UriKind.Relative);
        }

        this.ViewData["isCurrentUserTaskOwner"] = isCurrentUserTaskOwner;

        // TODO - Add Return URL to other viewmodels.
        viewModel.CommentsList = comments.Select(x => WebAppMapper.MapTodoTaskComment<TodoTaskComment, TodoTaskCommentVIewModel>(x));
        return this.View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> DetailsForAssignee(int id, int listId)
    {
        var apiResponse = await this.taskService.GetTodoTaskByIdAsync(id, listId);

        if (apiResponse == null)
        {
            return this.NotFound();
        }

        var viewModel = WebAppMapper.MapTodoTask<TodoTask, TodoTaskViewModel>(apiResponse);

        var comments = await this.commentService.GetCommentsForTaskAsync(id);

        if (comments == null)
        {
            return this.NotFound();
        }

        viewModel.CommentsList = comments.Select(x => WebAppMapper.MapTodoTaskComment<TodoTaskComment, TodoTaskCommentVIewModel>(x));

        return this.View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEdit(int id = 0, int listId = 0)
    {
        if (listId == 0)
        {
            return this.View("Error", new ErrorViewModel { RequestId = "Wrong Todo List Id specified.", ReturnUrl = new Uri($"/todolist/details?listId={id}", UriKind.Relative) });
        }

        this.ViewBag.AvailableTags = await this.tagService.GetAllTasksAsync();
        this.ViewBag.SelectedTagIds = new List<int>();

        var user = await this.GetCurrentUserAsync();

        if (id == 0)
        {
            return this.View(new TodoTaskViewModel()
            {
                // Setting Default values for the new TodoTask.
                // DueToDate will also be displayed in the Form in html file.
                TodoListName = "defaultName",
                DueToDate = DateTime.Now,
                Status = TodoTaskStatus.NotStarted,
                TodoListId = listId,
                AssigneeName = user.UserName,
                AssigneeId = user.UserIntId,
            });
        }

        var todo = await this.taskService.GetTodoTaskByIdAsync(id, listId);

        if (todo == null)
        {
            return this.View("Error", new ErrorViewModel { RequestId = $"Task with id {id} could not be found.", ReturnUrl = new Uri($"/todolist/details?listId={id}", UriKind.Relative) });
        }

        var model = WebAppMapper.MapTodoTask<TodoTask, TodoTaskViewModel>(todo);

        // Get actual tags for the editing Task.
        this.ViewBag.SelectedTagIds = new List<int>(todo!.TagList.Select(x => x.Id));

        // Get All users UseNames to display in select in Assignee field when editing the Assignee.
        var allUsers = await this.context.Users.Select(x => x.UserName).ToListAsync();
        this.ViewData["allUsers"] = allUsers;

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEdit(TodoTaskViewModel model, List<int> selectedTagIds)
    {
        if (!this.ModelState.IsValid)
        {
            this.ModelState.AddModelError(" ", "Not Valid");
            return this.View("Error", new ErrorViewModel { RequestId = $"Validation Errors occurred.", ReturnUrl = new Uri($"/todolist/details?listId={model.TodoListId}", UriKind.Relative) });
        }

        // Additional check if the user acutally exists since any input can still be entered.
        var assignedUser = await this.context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == model.AssigneeName.Trim().ToLower());
        if (assignedUser == null)
        {
            this.ModelState.AddModelError(model.AssigneeName, "User does not exist.");
            this.TempData["ErrorMessage"] = $"Validation failed, no such user - {model.AssigneeName}.";

            return this.RedirectToAction("CreateEdit", new { id = model.Id, listId = model.TodoListId });
        }

        var allTags = await this.tagService.GetAllTasksAsync();

        var user = await this.GetCurrentUserAsync();

        try
        {
            TodoTask todo = new TodoTask
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueToDate = model.DueToDate,
                AssigneeName = assignedUser.UserName,
                AssigneeId = assignedUser.UserIntId,    // TODO - Change USER id to another
                TodoListId = model.TodoListId,
                Status = model.Status,
                TagList = allTags.Where(x => selectedTagIds.Contains(x.Id)).ToList(),
            };

            var act = model.Id switch
            {
                0 => this.taskService.CreateTodoTaskAsync(todo),
                not 0 => this.taskService.UpdateTodoTaskAsync(todo),
            };

            var result = await act;

            if (result == null)
            {
                return this.RedirectToAction(actionName: "Details", controllerName: "TodoList", new { listId = model.TodoListId });
            }

            return this.RedirectToAction(actionName: "Details", controllerName: "TodoList", new { listId = model.TodoListId });
        }
        catch (ApplicationException ex)
        {
            this.ViewBag.ErrorMessage = ex.Message;
            return this.View("Error", new ErrorViewModel { RequestId = ex.Message, ReturnUrl = new Uri("/todolist", UriKind.Relative) });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, int listId)
    {
        var todo = await this.taskService.GetTodoTaskByIdAsync(id, listId);

        if (todo == null)
        {
            this.ViewBag.ErrorMessage = "No tasks found with this id.";
            return this.View("Error");
        }

        var result = await this.taskService.DeleteTodoTaskAsync(id, listId);

        if (!result)
        {
            this.ViewBag.ErrorMessage = "Error when deleting todotask.";
            return this.View("Error", new ErrorViewModel { RequestId = "Internal Error when deleting task", ReturnUrl = new Uri($"/todolist/details?listId={listId}", UriKind.Relative) });
        }

        return this.RedirectToAction(actionName: "Details", controllerName: "TodoList", new { listId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, int listId, TodoTaskStatus status)
    {
        var todo = await this.taskService.GetTodoTaskByIdAsync(id, listId);

        if (todo == null)
        {
            this.ViewBag.ErrorMessage = "No tasks found with this id.";
            return this.View("Error");
        }

        todo.Status = status;

        var result = await this.taskService.UpdateTodoTaskAsync(todo);

        if (result == null)
        {
            this.ViewBag.ErrorMessage = "Error when deleting todotask.";
            return this.View("Error", new ErrorViewModel { RequestId = "Internal Error when changing task status", ReturnUrl = new Uri($"/todolist/detailsforassignee/{id}listId={listId}", UriKind.Relative) });
        }

        return this.RedirectToAction("DetailsForAssignee", new { id, listId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int taskId, int listId, CreateTodoTaskCommentModel model)
    {
        var user = await this.GetCurrentUserAsync();

        var comment = new TodoTaskComment
        {
            Text = model.Text,
            DatePosted = DateTime.Now,
            UserId = user.UserIntId,
            TodoTaskId = taskId,
            UserName = user.UserName,
        };

        var request = await this.commentService.AddCommentAsync(comment);

        if (request == null)
        {
            this.ViewBag.ErrorMessage = "Error when adding comment.";
            return this.View("Error", new ErrorViewModel { RequestId = "Internal Error when changing task status", ReturnUrl = new Uri($"/todolist/detailsforassignee/{taskId}listId={listId}", UriKind.Relative) });
        }

        return this.RedirectToAction("DetailsForAssignee", new { id = taskId, listId });
    }

    [HttpGet]
    public async Task<IActionResult> DeleteComment(int id, int taskId, int listId)
    {
        var result = await this.commentService.DeleteByIdAsync(id);

        if (!result)
        {
            this.ViewBag.ErrorMessage = "Error when deleting comment.";
            return this.View("Error", new ErrorViewModel { RequestId = "Internal Error when deleting comment", ReturnUrl = new Uri($"/todotask/details/{taskId}", UriKind.Relative) });
        }

        return this.RedirectToAction(nameof(this.Details), new { id = taskId, listId });
    }
}
