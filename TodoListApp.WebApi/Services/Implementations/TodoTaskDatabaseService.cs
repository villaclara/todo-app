using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoTaskDatabaseService : ITodoTaskDatabaseService
{
    private readonly TodoListDbContext ctx;

    public TodoTaskDatabaseService(TodoListDbContext ctx)
    {
        this.ctx = ctx;
    }

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

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

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

    public async Task<TodoTask> GetByIdAsync(int id)
    {
        var entity = await this.ctx.TodoTasks.Include(x => x.TodoList).FirstOrDefaultAsync(t => t.Id == id);
        return entity == null
            ? throw new KeyNotFoundException($"TodoTask with Key {id} not found.")
            : new TodoTask()
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

    public Task<TodoTask> UpdateAsync(TodoTask todoTask)
    {
        throw new NotImplementedException();
    }
}
