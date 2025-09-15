using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{
    private readonly ITodoListDatabaseService todoListDatabaseService;

    public TodoListController(ITodoListDatabaseService todoListDatabaseService)
    {
        this.todoListDatabaseService = todoListDatabaseService;
    }

    [HttpGet("{userId:int}/lists")]
    public async Task<ActionResult<List<TodoListModel>>> GetAllTodosForUser(int userId)
    {
        var todos = await this.todoListDatabaseService.GetAllForUserAsync(userId);

        var result = todos.Select(x => new TodoListModel()
        {
            Id = x.Id,
            Description = x.Description,
            Title = x.Title,
            UserId = x.UserId,
        });

        return this.Ok(result);
    }

    [HttpGet("{userId:int}/lists/{id:int}")]
    public async Task<ActionResult<TodoListModel>> GetTodoListById(int userId, int id)
    {
        var todo = await this.todoListDatabaseService.GetByIdAsync(userId, id);

        if (todo == null)
        {
            return this.NotFound();
        }

        var result = new TodoListModel()
        {
            Id = todo.Id,
            Description = todo.Description,
            Title = todo.Title,
            UserId = todo.UserId,
        };

        return this.Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TodoListModel>> AddTodoList([FromBody] TodoListModel model)
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

            var result = await this.todoListDatabaseService.AddAsync(todo);

            if (result == null)
            {
                // TODO change to other error
                return this.BadRequest("internal error");
            }

            return this.Ok(result);
        }
        catch (Exception ex)
        {
            // TODO log exception
            return this.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error happened.");
        }
    }
}
