using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;
using TodoListApp.WebApi.Utility;

namespace TodoListApp.WebApi.Services.Implementations;

/// <summary>
/// Provides CRUD operations for <see cref="TodoTaskEntity"/> entities.
/// Encapsulates the database access using <see cref="TodoListDbContext"/>.
/// </summary>
public class TodoTaskDatabaseService : ITodoTaskDatabaseService
{
    private readonly TodoListDbContext ctx;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoTaskDatabaseService"/> class.
    /// </summary>
    /// <param name="ctx">Database context.</param>
    public TodoTaskDatabaseService(TodoListDbContext ctx)
    {
        this.ctx = ctx;
    }

    /// <inheritdoc/>
    public async Task<TodoTask> CreateAsync(TodoTask todoTask)
    {
        var entity = new TodoTaskEntity()
        {
            TodoListId = todoTask.TodoListId,
            Id = todoTask.Id,
            Assignee = todoTask.Assignee,
            CreatedAtDate = todoTask.CreatedAtDate,
            DueToDate = todoTask.DueToDate,
            Description = todoTask.Description,
            Title = todoTask.Title,
            Status = todoTask.Status,
        };

        var a = this.ctx.TodoTasks.Add(entity);
        _ = await this.ctx.SaveChangesAsync();

        // TODO - doing this to include the TodoList navigation property
        return await this.ctx.TodoTasks
            .Include(t => t.TodoList)
            .Where(t => t.Id == entity.Id)
            .Select(t => WebApiMapper.MapTodoTask<TodoTaskEntity, TodoTask>(t))
            .FirstAsync();
    }

    /// <inheritdoc/>
    public async Task<(int totalCount, List<TodoTask> todoTasks)> GetAllTodoTasksWithParamsAsync(int? todoListId, string? assignee, int? page, int? pageSize)
    {
        var query = this.ctx.TodoTasks.Include(x => x.TodoList).AsQueryable();

        if (!string.IsNullOrEmpty(assignee))
        {
            query = query.Where(t => t.Assignee == assignee);
        }

        if (todoListId.HasValue)
        {
            query = query.Where(t => t.TodoListId == todoListId.Value);
        }

        var totalCount = query.Count();

        if (page.HasValue && pageSize.HasValue)
        {
            query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var todos = await query
        .Select(x => WebApiMapper.MapTodoTask<TodoTaskEntity, TodoTask>(x))
        .ToListAsync();

        return (totalCount, todos);
    }

    /// <inheritdoc/>
    public async Task<TodoTask?> GetByIdAsync(int id)
    {
        var entity = await this.ctx.TodoTasks
            .Include(x => x.TodoList)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (entity == null)
        {
            return null;
        }

        return WebApiMapper.MapTodoTask<TodoTaskEntity, TodoTask>(entity);
    }

    /// <inheritdoc/>
    public async Task<TodoTask> UpdateAsync(TodoTask todoTask)
    {
        var entity = await this.ctx.TodoTasks.FindAsync(todoTask.Id)
            ?? throw new KeyNotFoundException($"TodoList with Id {todoTask.Id} not found.");

        if (!string.IsNullOrEmpty(todoTask.Title))
        {
            entity.Title = todoTask.Title;
        }

        if (!string.IsNullOrEmpty(todoTask.Description))
        {
            entity.Description = todoTask.Description;
        }

        if (!string.IsNullOrEmpty(todoTask.Assignee))
        {
            entity.Assignee = todoTask.Assignee;
        }

        // TODO - idk if its okay. This works, but we should allow user only specify the dateonly, not full datetime.
        if (todoTask.DueToDate.TimeOfDay.Hours == 0)
        {
            entity.DueToDate = todoTask.DueToDate;
        }

        if (todoTask.Status != entity.Status)
        {
            entity.Status = todoTask.Status;
        }

        _ = await this.ctx.SaveChangesAsync();
        await this.ctx.Entry(entity).Reference(e => e.TodoList).LoadAsync();

        return WebApiMapper.MapTodoTask<TodoTaskEntity, TodoTask>(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id)
    {
        var todoTask = await this.ctx.TodoTasks.FirstOrDefaultAsync(x => x.Id == id);
        if (todoTask == null)
        {
            return false;
        }

        _ = this.ctx.TodoTasks.Remove(todoTask);
        _ = await this.ctx.SaveChangesAsync();
        return true;
    }
}
