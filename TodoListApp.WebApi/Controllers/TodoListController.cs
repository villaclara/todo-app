using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Common;
using TodoListApp.Common.Models.TodoListModels;
using TodoListApp.Common.Parameters.Pagination;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;
using TodoListApp.WebApi.Utility;

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

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoListModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<TodoListModel>>> GetAllTodosForUser([FromQuery] int userId, [FromQuery] PaginationParameters pagination)
    {
        if (userId <= 0)
        {
            return this.BadRequest("UserId should be greater or equal to 1.");
        }

        pagination ??= new PaginationParameters();

        var (totalCount, todos) = await this.todoListDatabaseService.GetAllForUserAsync(userId, pagination);

        var result = todos.Select(x => WebApiMapper.MapTodoList<TodoList, TodoListModel>(x)).ToList();

        var paginationMetadata = new PaginationMetadata(totalCount, pagination.PageSize, pagination.PageNumber);

        var response = new ApiResponse<TodoListModel>
        {
            Data = result,
            Pagination = paginationMetadata,
        };

        return this.Ok(response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoListModel>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TodoListModel>>> GetTodoListById(int id, [FromQuery] int userId)
    {
        var todo = await this.todoListDatabaseService.GetByIdAsync(userId, id);

        if (todo == null)
        {
            this.logger.LogWarning("Todolist with id {@id} not found for user id {@userId}.", id, userId);
            return this.NotFound(new ApiResponse<TodoListModel>());
        }

        var result = WebApiMapper.MapTodoList<TodoList, TodoListModel>(todo);

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
                // return this.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error happened.");
                return this.StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse<TodoListModel>());
            }

            var mapped = WebApiMapper.MapTodoList<TodoList, TodoListModel>(result);

            // TODO - think if we need to return API response in Create, Update, Delete methods.
            return this.CreatedAtAction(actionName: nameof(this.GetTodoListById), new { userId = mapped.UserId, id = mapped.Id }, mapped);
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

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateTodoList(int id, [FromQuery] int userId, [FromBody] TodoListModel model)
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

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTodoList(int id, [FromQuery] int userId)
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
