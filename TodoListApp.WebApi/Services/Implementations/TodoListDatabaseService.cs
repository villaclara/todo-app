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
    public async Task<TodoList?> CreateAsync(TodoList todo)
    {
        var entity = new TodoListEntity()
        {
            Title = todo.Title,
            Description = todo.Description,
            UserId = todo.UserId,
        };
        _ = this.ctx.TodoLists.Add(entity);
        _ = await this.ctx.SaveChangesAsync();
        return new TodoList() { Id = entity.Id, UserId = entity.UserId, Title = entity.Title, Description = entity.Description };
    }

    /// <inheritdoc/>
    public async Task<List<TodoList>> GetAllForUserAsync(int userId)
    {
        return await this.ctx.TodoLists.Where(x => x.UserId == userId)
            .Select(x => new TodoList() { Id = x.Id, Description = x.Description, Title = x.Title, UserId = x.UserId })
            .ToListAsync() ?? new List<TodoList>();
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<TodoList> UpdateAsync(TodoList todo)
    {
        var entity = await this.ctx.TodoLists.FindAsync(todo.Id)
            ?? throw new KeyNotFoundException($"TodoList with Id {todo.Id} not found.");

        entity.Title = todo.Title;
        entity.Description = todo.Description;

        _ = await this.ctx.SaveChangesAsync();
        return new TodoList()
        {
            Id = todo.Id,
            Description = todo.Description,
            Title = todo.Title,
            UserId = todo.UserId,
        };
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteByIdAsync(int userId, int todoListId)
    {
        var todo = await this.ctx.TodoLists.FirstOrDefaultAsync(x => x.Id == todoListId && x.UserId == userId);
        if (todo == null)
        {
            return false;
        }

        _ = this.ctx.TodoLists.Remove(todo);
        _ = await this.ctx.SaveChangesAsync();
        return true;
    }
}
