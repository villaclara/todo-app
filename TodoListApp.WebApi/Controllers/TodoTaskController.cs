using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Common;
using TodoListApp.Common.Models.Enums;
using TodoListApp.Common.Models.TodoListModels;
using TodoListApp.Common.Models.TodoTaskModels;
using TodoListApp.Common.Parameters.Filtering;
using TodoListApp.Common.Parameters.Pagination;
using TodoListApp.Common.Parameters.Sorting;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;
using TodoListApp.WebApi.Utility;

namespace TodoListApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoTaskController : ControllerBase
{
    private readonly ITodoTaskDatabaseService taskService;
    private readonly ILogger<TodoTaskController> logger;

    public TodoTaskController(ITodoTaskDatabaseService todoTaskService, ILogger<TodoTaskController> logger)
    {
        this.taskService = todoTaskService;
        this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoTaskModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<TodoTaskModel>>> GetAllTasksForList(
        [FromQuery] int? listId,
        [FromQuery] int? assigneeId,
        [FromQuery] PaginationParameters pagination,
        [FromQuery] TodoTaskAssigneeFilter filter,
        [FromQuery] TodoTaskStatusFilterOption statusFilterOption = TodoTaskStatusFilterOption.NotCompleted,
        [FromQuery] TaskSortingOptions sorting = TaskSortingOptions.DueDateAsc)
    {
        if (listId.HasValue && listId <= 0)
        {
            return this.BadRequest("ListId should be greater or equal to 1.");
        }

        if (pagination == null)
        {
            pagination = new PaginationParameters();
        }

        // TODO - check if the list exists by listId before getting data next.
        var todoTasks = await this.taskService.GetAllTodoTasksWithParamsAsync(listId, assigneeId, pagination, filter, statusFilterOption, sorting);
        var result = todoTasks.todoTasks.Select(x => WebApiMapper.MapTodoTask<TodoTask, TodoTaskModel>(x)).ToList();

        var paginationMetadata = new PaginationMetadata(todoTasks.totalCount, pagination.PageSize, pagination.PageNumber);

        var response = new ApiResponse<TodoTaskModel>()
        {
            Data = result,
            Pagination = paginationMetadata,
        };

        return this.Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoTaskModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TodoTaskModel>>> GetTaskById(int id, [FromQuery] int listId)
    {
        _ = listId;
        var todotask = await this.taskService.GetByIdAsync(id);

        if (todotask == null)
        {
            this.logger.LogWarning("Todotask with id {@id} not found for list id {@userId}.", id, listId);
            return this.NotFound(new ApiResponse<TodoListModel>());
        }

        var result = WebApiMapper.MapTodoTask<TodoTask, TodoTaskModel>(todotask);

        var response = new ApiResponse<TodoTaskModel>()
        {
            Data = new List<TodoTaskModel> { result },
        };

        return this.Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTodoTask([FromBody] CreateTodoTaskModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            var todoTask = new TodoTask()
            {
                Title = model.Title,
                CreatedByUserId = model.CreatedByUserId,
                CreatedByUserName = model.CreatedByUserName,
                AssigneeName = model.AssigneeName,
                AssigneeId = model.AssigneeId,
                CreatedAtDate = DateTime.UtcNow,
                DueToDate = model.DueToDate,
                Description = model.Description,
                Status = TodoTaskStatus.NotStarted,
                TodoListId = model.TodoListId,
                TagList = model.TagList.Select(x => new TodoTaskTag { Id = x.Id, Title = x.Title }).ToList(),
            };

            var result = await this.taskService.CreateAsync(todoTask);

            if (result == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error happened.");
            }

            var mapped = WebApiMapper.MapTodoTask<TodoTask, TodoTaskModel>(result);

            return this.CreatedAtAction(actionName: nameof(this.GetTaskById), new { listId = mapped.TodoListId, id = mapped.Id }, mapped);
        }
        catch (DbUpdateException dbEx)
        {
            this.logger.LogError("{@Method} - Exception - {@ex}.", nameof(this.AddTodoTask), dbEx.Message);

            // e.g. SQL unique constraint violation
            return this.Conflict("Database update failed. A list with the same name may already exist.");
        }
        catch (ArgumentException argEx)
        {
            return this.BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            this.logger.LogError("{@Method} - Exception thrown - {@ex}.", nameof(this.AddTodoTask), ex.Message);
            throw;
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTodoTask(int id, [FromQuery] int listId, [FromBody] UpdateTodoTaskModel model)
    {
        // Check if the correct value is passed as enum.
        if (!Enum.IsDefined(model.Status))
        {
            return this.BadRequest("Wrong Task Status.");
        }

        var todoTask = new TodoTask()
        {
            Id = id,
            Title = model.Title ?? string.Empty,
            Description = model.Description ?? string.Empty,
            DueToDate = model.DueToDate ?? DateTime.UtcNow,
            Status = model.Status,
            AssigneeName = model.AssigneeName ?? string.Empty,
            AssigneeId = model.AssigneeId,
            TodoListId = listId,
            TagList = model.TagList.Select(x => new TodoTaskTag { Id = x.Id, Title = x.Title }).ToList(),
        };

        try
        {
            var result = await this.taskService.UpdateAsync(todoTask);
            return this.NoContent();
        }
        catch (KeyNotFoundException knfEx)
        {
            this.logger.LogError("{@Method} - {@ex}.", nameof(this.UpdateTodoTask), knfEx.Message);
            return this.NotFound();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            this.logger.LogError("{@Method} - Exception - {@ex}.", nameof(this.UpdateTodoTask), dbUpdateEx.Message);

            // e.g. SQL unique constraint violation
            return this.Conflict("Database update failed.");
        }
        catch (Exception ex)
        {
            this.logger.LogError("{@Method} - Exception thrown - {@ex}.", nameof(this.UpdateTodoTask), ex.Message);
            throw;
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodoTask(int id, [FromQuery] int listId)
    {
        if (listId == 0)
        {
            this.logger.LogWarning("Wrong list Id passed.");
            return this.BadRequest();
        }

        // TODO - It also looks not very good as the false is also called when the todolist does not belong to the user.
        var result = await this.taskService.DeleteAsync(id, listId);
        if (!result)
        {
            this.logger.LogWarning("{@Method} - TodoTask with {@id} not deleted due to not found.", nameof(this.DeleteTodoTask), id);
            return this.NotFound();
        }

        return this.NoContent();
    }
}
