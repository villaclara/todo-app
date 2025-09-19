using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models.TodoTaskModels;
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

    [HttpGet("{listId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTasksForList(int listId)
    {
        var todoTasks = await this.taskService.GetAllForTodoListAsync(listId);
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

        return this.Ok(result);
    }

    [HttpGet("{listId:int}/task/{id:int}")]
    public async Task<IActionResult> GetTaskById(int listId, int id)
    {
        var a = listId;
        var todotask = await this.taskService.GetByIdAsync(id);
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

        return this.Ok(result);
    }

    [HttpPost]
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
}
