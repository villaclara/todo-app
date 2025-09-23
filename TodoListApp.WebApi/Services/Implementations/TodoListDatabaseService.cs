using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Services.Implementations;

/// <summary>
/// Provides CRUD operations for <see cref="TodoListEntity"/> entities.
/// Encapsulates the database access using <see cref="TodoListDbContext"/>.
/// </summary>
public class TodoListDatabaseService : ITodoListDatabaseService
{
    private readonly TodoListDbContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoListDatabaseService"/> class.
    /// </summary>
    /// <param name="ctx">Database context.</param>
    public TodoListDatabaseService(TodoListDbContext context)
    {
        this.ctx = context;
    }

    /// <inheritdoc/>
    public async Task<TodoList> CreateAsync(TodoList todo)
    {
        var entity = new TodoListEntity()
        {
            Title = todo.Title,
            Description = todo.Description,
            UserId = todo.UserId,
        };
        _ = this.ctx.TodoLists.Add(entity);
        _ = await this.ctx.SaveChangesAsync();

        return new TodoList()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Title = entity.Title,
            Description = entity.Description,
        };
    }

    /// <inheritdoc/>
    public async Task<List<TodoList>> GetAllForUserAsync(int userId, int pageNumber = 1, int pageSize = 10)
    {
        var items = await this.ctx.TodoLists.Where(x => x.UserId == userId).OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new TodoList()
            {
                Id = x.Id,
                Description = x.Description,
                Title = x.Title,
                UserId = x.UserId,
            })
            .ToListAsync();

        return items;
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

        if (!string.IsNullOrEmpty(todo.Title))
        {
            entity.Title = todo.Title;
        }

        if (!string.IsNullOrEmpty(todo.Description))
        {
            entity.Description = todo.Description;
        }

        _ = await this.ctx.SaveChangesAsync();
        return new TodoList()
        {
            Id = entity.Id,
            Description = entity.Description,
            Title = entity.Title,
            UserId = entity.UserId,
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
