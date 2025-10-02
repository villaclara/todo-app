using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;
using TodoListApp.WebApi.Utility;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoTaskCommentDatabaseService : ITodoTaskCommentDatabaseService
{
    private readonly TodoListDbContext ctx;

    public TodoTaskCommentDatabaseService(TodoListDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<TodoTaskComment> CreateAsync(TodoTaskComment comment)
    {
        var entity = WebApiMapper.MapTodoTaskComment<TodoTaskComment, TodoTaskCommentEntity>(comment);

        _ = this.ctx.TodoTaskComments.Add(entity);
        _ = await this.ctx.SaveChangesAsync();

        return WebApiMapper.MapTodoTaskComment<TodoTaskCommentEntity, TodoTaskComment>(entity);
    }

    public async Task<List<TodoTaskComment>> GetCommentsForTaskByIdAsync(int taskId)
    {
        return await this.ctx.TodoTaskComments
            .Where(x => x.TodoTaskId == taskId)
            .OrderBy(x => x.DatePosted)
            .Select(x => WebApiMapper.MapTodoTaskComment<TodoTaskCommentEntity, TodoTaskComment>(x))
            .ToListAsync() ?? new List<TodoTaskComment>();
    }

    public async Task<bool> DeleteByIdAsync(int commentId)
    {
        var entity = await this.ctx.TodoTaskComments.FirstOrDefaultAsync(x => x.Id == commentId);

        if (entity == null)
        {
            return false;
        }

        _ = this.ctx.TodoTaskComments.Remove(entity);
        _ = await this.ctx.SaveChangesAsync();
        return true;
    }

    public async Task<TodoTaskComment> UpdateAsync(TodoTaskComment comment)
    {
        var entity = await this.ctx.TodoTaskComments.FindAsync(comment.Id)
            ?? throw new KeyNotFoundException($"comment with Id {comment.Id} not found.");

        if (!string.IsNullOrEmpty(comment.Text))
        {
            entity.Text = comment.Text;
        }

        _ = await this.ctx.SaveChangesAsync();
        return WebApiMapper.MapTodoTaskComment<TodoTaskCommentEntity, TodoTaskComment>(entity);
    }

    public async Task<bool> DeleteAllCommentsForTaskId(int taskId)
    {
        var entities = this.ctx.TodoTaskComments.Where(x => x.TodoTaskId == taskId);

        if (entities == null)
        {
            return false;
        }

        this.ctx.TodoTaskComments.RemoveRange(entities);
        _ = await this.ctx.SaveChangesAsync();
        return true;
    }
}
