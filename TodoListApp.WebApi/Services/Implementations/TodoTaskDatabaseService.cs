using Microsoft.EntityFrameworkCore;
using TodoListApp.Common.Parameters.Filtering;
using TodoListApp.Common.Parameters.Pagination;
using TodoListApp.Common.Parameters.Sorting;
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
            AssigneeName = todoTask.AssigneeName,
            AssigneeId = todoTask.AssigneeId,
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
    public async Task<(int totalCount, List<TodoTask> todoTasks)> GetAllTodoTasksWithParamsAsync(
        int? todoListId,
        int? assigneeId,
        PaginationParameters pagination,
        TodoTaskAssigneeFilter filter,
        TaskSortingOptions sorting = TaskSortingOptions.CreatedDateDesc)
    {
        var query = this.ctx.TodoTasks.Include(x => x.TodoList).AsQueryable();

        if (assigneeId.HasValue)
        {
            query = query.Where(t => t.AssigneeId == assigneeId);
        }

        // Apply TodoTaskAssigneeFilter filters
        if (filter != null)
        {
            // Date creation filters
            if (filter.CreatedAfter.HasValue)
            {
                query = query.Where(t => t.CreatedAtDate >= filter.CreatedAfter.Value);
            }

            if (filter.CreatedBefore.HasValue)
            {
                query = query.Where(t => t.CreatedAtDate <= filter.CreatedBefore.Value);
            }

            // Due date filters
            if (filter.DueAfter.HasValue)
            {
                query = query.Where(t => t.DueToDate >= filter.DueAfter.Value);
            }

            if (filter.DueBefore.HasValue)
            {
                query = query.Where(t => t.DueToDate <= filter.DueBefore.Value);
            }

            // Status filter
            if (filter.Status.HasValue)
            {
                query = query.Where(t => t.Status == filter.Status.Value);
            }

            // TodoList filters
            if (filter.TodoListId.HasValue)
            {
                query = query.Where(t => t.TodoListId == filter.TodoListId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.TodoListNameContains))
            {
                query = query.Where(t => t.TodoList.Title.Contains(filter.TodoListNameContains));
            }
        }

        query = sorting switch
        {
            TaskSortingOptions.CreatedDateAsc => query.OrderBy(x => x.CreatedAtDate),
            TaskSortingOptions.CreatedDateDesc => query.OrderByDescending(x => x.CreatedAtDate),
            TaskSortingOptions.TodoListNameAsc => query.OrderBy(x => x.TodoList.Title),
            TaskSortingOptions.TodoListNameDesc => query.OrderByDescending(x => x.TodoList.Title),
            TaskSortingOptions.DueDateAsc => query.OrderBy(x => x.DueToDate),
            TaskSortingOptions.DueDateDesc => query.OrderByDescending(x => x.DueToDate),
            TaskSortingOptions.TaskStatusAsc => query.OrderBy(x => x.Status),
            TaskSortingOptions.TaskStatusDesc => query.OrderByDescending(x => x.Status),
            _ => query.OrderByDescending(x => x.CreatedAtDate),
        };

        if (todoListId.HasValue)
        {
            query = query.Where(t => t.TodoListId == todoListId.Value);
        }

        var totalCount = query.Count();

        query = query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);

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

        if (!string.IsNullOrEmpty(todoTask.AssigneeName))
        {
            entity.AssigneeName = todoTask.AssigneeName;
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
    public async Task<bool> DeleteAsync(int id, int listId)
    {
        var todoTask = await this.ctx.TodoTasks.FirstOrDefaultAsync(x => x.Id == id && x.TodoListId == listId);
        if (todoTask == null)
        {
            return false;
        }

        _ = this.ctx.TodoTasks.Remove(todoTask);
        _ = await this.ctx.SaveChangesAsync();
        return true;
    }
}
