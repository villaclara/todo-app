using Microsoft.AspNetCore.Mvc;
using TodoListApp.Common.Models.Enums;
using TodoListApp.Common.Models.TodoTaskCommentModels;
using TodoListApp.Common.Parameters.Filtering;
using TodoListApp.Common.Parameters.Pagination;
using TodoListApp.Common.Parameters.Sorting;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;
using TodoListApp.WebApp.Utility;

namespace TodoListApp.WebApp.Controllers;
public class TodoTaskController : Controller
{
    private readonly ITodoTaskWebApiService taskService;
    private readonly ITodoTaskTagWebApiService tagService;
    private readonly ITodoTaskCommentWebApiService commentService;
    private readonly ITodoListWebApiService listService;

    public TodoTaskController(ITodoTaskWebApiService taskService, ITodoTaskTagWebApiService tagService, ITodoTaskCommentWebApiService commentService, ITodoListWebApiService listService)
    {
        this.taskService = taskService;
        this.tagService = tagService;
        this.commentService = commentService;
        this.listService = listService;
    }

    public async Task<IActionResult> Index(
        PaginationParameters pagination,
        TodoTaskAssigneeFilter filter,
        TodoTaskStatusFilterOption statusFilterOption = TodoTaskStatusFilterOption.NotCompleted,
        TaskSortingOptions sorting = TaskSortingOptions.DueDateAsc)
    {
        var apiResponse = await this.taskService.GetPagedTodoTasksByAssigneeAsync(1, pagination, filter, statusFilterOption, sorting); // TODO - Assignee Id put

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
    public async Task<IActionResult> Details(int id, int listId)
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

        var list = await this.listService.GetTodoListByIdAsync(viewModel.TodoListId, 1); // TODO - user is
        if (list == null)
        {
            return this.NotFound();
        }

        // TODO - Add check if user owner when editing
        var isCurrentUserTaskOwner = list.UserId == 1; // TODO - user id
        this.ViewData["isCurrentUserTaskOwner"] = isCurrentUserTaskOwner;

        // TODO - Add Return URL to viewmodels.
        viewModel.CommentsList = comments.Select(x => WebAppMapper.MapTodoTaskComment<TodoTaskComment, TodoTaskCommentVIewModel>(x));
        viewModel.ReturnUrl = new Uri($"/todolist/details?listId={viewModel.TodoListId}", UriKind.Relative);
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
                AssigneeName = "user", // TODO - Change to user
                AssigneeId = 1,
            });
        }

        var todo = await this.taskService.GetTodoTaskByIdAsync(id, listId);   // TODO - id userId

        if (todo == null)
        {
            return this.View("Error", new ErrorViewModel { RequestId = $"Task with id {id} could not be found.", ReturnUrl = new Uri($"/todolist/details?listId={id}", UriKind.Relative) });
        }

        var model = WebAppMapper.MapTodoTask<TodoTask, TodoTaskViewModel>(todo);

        this.ViewBag.SelectedTagIds = new List<int>(todo!.TagList.Select(x => x.Id));

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

        var allTags = await this.tagService.GetAllTasksAsync();

        try
        {
            TodoTask todo = new TodoTask
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueToDate = model.DueToDate,
                AssigneeName = model.AssigneeName ?? "bruh", // TODO - assignee and use Mapper
                AssigneeId = 1, // TODO - should get user Id from Context or whatever
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
        var todo = await this.taskService.GetTodoTaskByIdAsync(id, listId);  // TODO - id

        if (todo == null)
        {
            this.ViewBag.ErrorMessage = "No tasks found with this id.";
            return this.View("Error");
        }

        var result = await this.taskService.DeleteTodoTaskAsync(id, listId); // TODO - id

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
        var todo = await this.taskService.GetTodoTaskByIdAsync(id, listId);  // TODO - id

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
        var comment = new TodoTaskComment
        {
            Text = model.Text,
            DatePosted = DateTime.Now,
            UserId = 1, // TODO - user Id
            TodoTaskId = taskId,
            UserName = "user", // TODO - username
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
