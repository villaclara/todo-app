using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Models.TodoListModels;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ITodoListDatabaseService todoListDatabaseService;
    private readonly ILogger<TodoListController> logger;

    public TodoListController(ITodoListDatabaseService todoListDatabaseService, ILogger<TodoListController> logger)
    {
        this.todoListDatabaseService = todoListDatabaseService;
        this.logger = logger;
    }

    [HttpGet("{userId:int}/lists")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoListModel>))]
    public async Task<ActionResult<ApiResponse<TodoListModel>>> GetAllTodosForUser(int userId)
    {
        var todos = await this.todoListDatabaseService.GetAllForUserAsync(userId);

        var result = todos.Select(x => new TodoListModel
        {
            Id = x.Id,
            Description = x.Description,
            Title = x.Title,
            UserId = x.UserId,
        });

        var response = new ApiResponse<TodoListModel>()
        {
            Data = result,
        };

        return this.Ok(response);
    }

    [HttpGet("{userId:int}/lists/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoListModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TodoListModel>>> GetTodoListById(int userId, int id)
    {
        var todo = await this.todoListDatabaseService.GetByIdAsync(userId, id);

        if (todo == null)
        {
            this.logger.LogWarning("Todolist with id {@id} not found for user id {@userId}.", id, userId);
            return this.NotFound(new ApiResponse<TodoListModel>());
        }

        var result = new TodoListModel()
        {
            Id = todo.Id,
            Description = todo.Description,
            Title = todo.Title,
            UserId = todo.UserId,
        };

        var response = new ApiResponse<TodoListModel>()
        {
            Data = new List<TodoListModel> { result },
        };

        return this.Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTodoList([FromBody] CreateTodoListModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            var todo = new TodoList()
            {
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
            };

            var result = await this.todoListDatabaseService.CreateAsync(todo);

            if (result == null)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error happened.");
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<TodoListModel>());
            }

            // TODO - think if we need to return API response in Create, Update, Delete methods.
            return this.CreatedAtAction(actionName: nameof(this.GetTodoListById), new { userId = result.UserId, id = result.Id }, result);
        }
        catch (DbUpdateException dbEx)
        {
            this.logger.LogError("{@Method} - Exception - {@ex}.", nameof(this.AddTodoList), dbEx.Message);

            // e.g. SQL unique constraint violation
            return this.Conflict("Database update failed. A list with the same name may already exist.");
        }
        catch (ArgumentException argEx)
        {
            return this.BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            this.logger.LogError("{@Method} - Exception thrown - {@ex}.", nameof(this.AddTodoList), ex.Message);
            throw;
        }
    }

    [HttpPut("{userId:int}/lists/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTodoList(int userId, int id, [FromBody] TodoListModel model)
    {
        // TODO - Not sure if this is correct. As I dont know if I want to expose the Id to TodoListModel.
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        var todolist = new TodoList()
        {
            UserId = userId,
            Id = id,
            Title = model.Title,
            Description = model.Description,
        };

        try
        {
            var result = await this.todoListDatabaseService.UpdateAsync(todolist);
            return this.NoContent();
        }
        catch (KeyNotFoundException knfEx)
        {
            this.logger.LogError("{@Method} - {@ex}.", nameof(this.UpdateTodoList), knfEx.Message);
            return this.NotFound();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            this.logger.LogError("{@Method} - Exception - {@ex}.", nameof(this.UpdateTodoList), dbUpdateEx.Message);

            // e.g. SQL unique constraint violation
            return this.Conflict("Database update failed. A list with the same name may already exist.");
        }
        catch (Exception ex)
        {
            this.logger.LogError("{@Method} - Exception thrown - {@ex}.", nameof(this.UpdateTodoList), ex.Message);
            throw;
        }
    }

    [HttpDelete("{userId:int}/lists/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodoList(int userId, int id)
    {
        // TODO - It also looks not very good as the false is also called when the todolist does not belong to the user.
        var result = await this.todoListDatabaseService.DeleteByIdAsync(userId, id);
        if (!result)
        {
            this.logger.LogWarning("{@Method} - Todolist with {@id} not deleted due to not found.", nameof(this.DeleteTodoList), id);
            return this.NotFound();
        }

        return this.NoContent();
    }
}
