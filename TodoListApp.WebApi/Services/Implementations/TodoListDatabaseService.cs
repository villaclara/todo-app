using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly TodoListDbContext ctx;

    public TodoListDatabaseService(TodoListDbContext context)
    {
        this.ctx = context;
    }

    /// <inheritdoc/>
    public async Task<List<TodoList>> GetAllForUserAsync(int userId)
    {
        return await this.ctx.TodoLists.Where(x => x.UserId == userId).Select(x => new TodoList() { }).ToListAsync() ?? new List<TodoList>();
    }
}
