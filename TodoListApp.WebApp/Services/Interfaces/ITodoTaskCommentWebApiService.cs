using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoTaskCommentWebApiService
{
    Task<IEnumerable<TodoTaskComment>> GetCommentsForTaskAsync(int taskId);

    Task<TodoTaskComment?> AddCommentAsync(TodoTaskComment comment);

    Task<TodoTaskComment> UpdateCommentAsync(TodoTaskComment comment);

    Task<bool> DeleteByIdAsync(int commentId);

    Task<bool> DeleteAllCommentsForTaskIdAsync(int taskId);
}
