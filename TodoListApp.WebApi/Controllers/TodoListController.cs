using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApi.Models;
using TodoListApp.WebApi.Services.Interfaces;

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

    [HttpGet("{id:int}")]
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
}
