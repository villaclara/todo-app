using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoTaskCommentWebApiService
{
    Task<IEnumerable<TodoTaskComment>> GetCommentsForTask(int taskId);

    Task<TodoTaskComment?> AddComment(TodoTaskComment comment);

    Task<bool> DeleteByIdAsync(int commentId);

    Task<bool> DeleteAllCommentsForTaskId(int taskId);
}
