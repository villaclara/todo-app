using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;

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
            Status = todoTask.TaskStatus,
        };

        var a = this.ctx.TodoTasks.Add(entity);
        _ = await this.ctx.SaveChangesAsync();

        // TODO - doing this to include the TodoList navigation property
        return await this.ctx.TodoTasks.Where(t => t.Id == entity.Id).Select(t => new TodoTask()
        {
            TodoListId = t.TodoListId,
            Id = t.Id,
            Assignee = t.Assignee,
            CreatedAtDate = entity.CreatedAtDate,
            DueToDate = t.DueToDate,
            Description = t.Description,
            Title = t.Title,
            TaskStatus = t.Status,
            TodoListName = t.TodoList.Title,
        }).FirstAsync();
    }

    /// <inheritdoc/>
    public async Task<List<TodoTask>> GetAllForTodoListAsync(int todoListId) =>
        await this.ctx.TodoTasks.Where(x => x.TodoListId == todoListId).Select(x => new TodoTask()
        {
            TodoListId = todoListId,
            Id = x.Id,
            Assignee = x.Assignee,
            CreatedAtDate = x.CreatedAtDate,
            DueToDate = x.DueToDate,
            Description = x.Description,
            Title = x.Title,
            TaskStatus = x.Status,
            TodoListName = x.TodoList.Title,
        }).ToListAsync() ?? new List<TodoTask>();

    /// <inheritdoc/>
    public async Task<TodoTask?> GetByIdAsync(int id)
    {
        var entity = await this.ctx.TodoTasks.Include(x => x.TodoList).FirstOrDefaultAsync(t => t.Id == id);

        if (entity == null)
        {
            return null;
        }

        return new TodoTask()
        {
            TodoListId = entity.TodoListId,
            Id = entity.Id,
            Assignee = entity.Assignee,
            CreatedAtDate = entity.CreatedAtDate,
            DueToDate = entity.DueToDate,
            Description = entity.Description,
            Title = entity.Title,
            TaskStatus = entity.Status,
            TodoListName = entity.TodoList.Title,
        };
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

        _ = await this.ctx.SaveChangesAsync();
        await this.ctx.Entry(entity).Reference(e => e.TodoList).LoadAsync();

        return new TodoTask()
        {
            TodoListId = entity.TodoListId,
            Id = entity.Id,
            Assignee = entity.Assignee,
            CreatedAtDate = entity.CreatedAtDate,
            DueToDate = entity.DueToDate,
            Description = entity.Description,
            Title = entity.Title,
            TaskStatus = entity.Status,
            TodoListName = entity.TodoList.Title,
        };
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
