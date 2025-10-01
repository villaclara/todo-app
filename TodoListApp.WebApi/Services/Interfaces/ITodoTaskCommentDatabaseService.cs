using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoTaskCommentDatabaseService
{
    Task<TodoTaskComment> CreateAsync(TodoTaskComment comment);

    Task<List<TodoTaskComment>> GetCommentsForTaskByIdAsync(int taskId);

    Task<bool> DeleteByIdAsync(int commentId);

    Task<bool> DeleteAllCommentsForTaskId(int taskId);
}
