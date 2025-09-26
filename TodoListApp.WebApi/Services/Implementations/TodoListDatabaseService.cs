using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;
using TodoListApp.WebApi.Utility;

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
        //var entity = new TodoListEntity()
        //{
        //    Title = todo.Title,
        //    Description = todo.Description,
        //    UserId = todo.UserId,
        //};

        var entity = Mapper.MapTodoList<TodoList, TodoListEntity>(todo);

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
    public async Task<(int totalCount, List<TodoList> todos)> GetAllForUserAsync(int userId, int pageNumber = 1, int pageSize = 10)
    {
        var todos = await this.ctx.TodoLists.Where(x => x.UserId == userId).ToListAsync();
        var totalCount = todos.Count;
        var items = todos
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => Mapper.MapTodoList<TodoListEntity, TodoList>(x))
            .ToList();

        return (totalCount, items);
    }

    /// <inheritdoc/>
    public async Task<TodoList?> GetByIdAsync(int userId, int todoListId)
    {
        var entity = await this.ctx.TodoLists.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == todoListId);

        if (entity == null)
        {
            return null;
        }

        var result = Mapper.MapTodoList<TodoListEntity, TodoList>(entity);

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
        return Mapper.MapTodoList<TodoListEntity, TodoList>(entity);
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
