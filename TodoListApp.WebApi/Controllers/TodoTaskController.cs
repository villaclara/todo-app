using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Models.TodoListModels;
using Shared.Models.TodoTaskModels;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;

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
    public async Task<ActionResult<ApiResponse<TodoTaskModel>>> GetAllTasksForList([FromQuery] int? listId, [FromQuery] string? assignee)
    {
        if (listId.HasValue && listId <= 0)
        {
            return this.BadRequest("ListId should be greater or equal to 1.");
        }

        var todoTasks = await this.taskService.GetAllTodoTasksWithParamsAsync(listId, assignee);
        var result = todoTasks.Select(x => new TodoTaskModel
        {
            Id = x.Id,
            Assignee = x.Assignee,
            CreatedAtDate = x.CreatedAtDate,
            Description = x.Description,
            DueToDate = x.DueToDate,
            IsOverdue = x.IsOverdue,
            Status = x.TaskStatus.ToString(),
            Title = x.Title,
            TodoListName = x.TodoListName,
        });

        var response = new ApiResponse<TodoTaskModel>()
        {
            Data = result,
        };

        return this.Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoTaskModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TodoTaskModel>>> GetTaskById(int id, [FromQuery] int listId)
    {
        var a = listId;
        var todotask = await this.taskService.GetByIdAsync(id);

        if (todotask == null)
        {
            this.logger.LogWarning("Todotask with id {@id} not found for list id {@userId}.", id, listId);
            return this.NotFound(new ApiResponse<TodoListModel>());
        }

        var result = new TodoTaskModel()
        {
            Id = todotask.Id,
            Assignee = todotask.Assignee,
            CreatedAtDate = todotask.CreatedAtDate,
            Description = todotask.Description,
            DueToDate = todotask.DueToDate,
            IsOverdue = todotask.IsOverdue,
            Status = todotask.TaskStatus.ToString(),
            Title = todotask.Title,
            TodoListName = todotask.TodoListName,
            TodoListId = todotask.TodoListId,
        };

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
                Assignee = "user",  // TODO - Current user should be assigned
                CreatedAtDate = DateTime.UtcNow,
                DueToDate = model.DueToDate,
                Description = model.Description,
                TaskStatus = Entities.Enums.TodoTaskStatus.NotStarted,
                TodoListId = model.TodoListId,
            };

            var result = await this.taskService.CreateAsync(todoTask);

            if (result == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error happened.");
            }

            return this.CreatedAtAction(actionName: nameof(this.GetTaskById), new { listId = result.TodoListId, id = result.Id }, result);
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
    public async Task<IActionResult> UpdateTodoTask(int id, [FromQuery] int listId, [FromBody] UpdateTodoTaskModel model)
    {
        // TODO - Not sure if this is correct. As I dont know if I want to expose the Id to TodoListModel.
        //if (id != model.Id)
        //{
        //    return this.BadRequest();
        //}

        var todoTask = new TodoTask()
        {
            Id = id,
            Title = model.Title ?? string.Empty,
            Description = model.Description ?? string.Empty,
            DueToDate = model.DueToDate ?? DateTime.UtcNow,
            TaskStatus = Entities.Enums.TodoTaskStatus.InProgress,
            Assignee = model.Assignee ?? string.Empty,
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodoTask(int id, [FromQuery] int listId)
    {
        // TODO - It also looks not very good as the false is also called when the todolist does not belong to the user.
        var result = await this.taskService.DeleteAsync(id);
        if (!result)
        {
            this.logger.LogWarning("{@Method} - TodoTask with {@id} not deleted due to not found.", nameof(this.DeleteTodoTask), id);
            return this.NotFound();
        }

        return this.NoContent();
    }
}
