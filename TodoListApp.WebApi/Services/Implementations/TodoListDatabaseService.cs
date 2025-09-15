using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Entities;
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
        return await this.ctx.TodoLists.Where(x => x.UserId == userId).Select(x => new TodoList() { Id = x.Id, Description = x.Description, Title = x.Title, UserId = x.UserId }).ToListAsync() ?? new List<TodoList>();
    }

    public async Task<TodoList?> GetByIdAsync(int userId, int todoListId)
    {
        var entity = await this.ctx.TodoLists.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == todoListId);

        if (entity == null)
        {
            return null;
        }

        var result = new TodoList()
        {
            Id = entity.Id,
            Description = entity.Description,
            Title = entity.Title,
            UserId = entity.UserId,
        };

        return result;
    }

    public async Task<TodoList?> AddAsync(TodoList todo)
    {
        var entity = new TodoListEntity()
        {
            Title = todo.Title,
            Description = todo.Description,
            UserId = todo.UserId,
        };
        _ = await this.ctx.TodoLists.AddAsync(entity);
        _ = await this.ctx.SaveChangesAsync();
        return new TodoList() { Id = entity.Id, UserId = entity.UserId, Title = entity.Title, Description = entity.Description };
    }
}
