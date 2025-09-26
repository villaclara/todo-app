using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoTaskWebApiService
{
    Task<IEnumerable<TodoTask>> GetTodoTasksAsync(int listId);

    Task<PagedResults<TodoTask>> GetPagedTodoTasksByListAsync(int listId, int page, int pageSize);

    Task<PagedResults<TodoTask>> GetPagedTodoTasksByAssigneeAsync(int assigneeId, int page, int pageSize);

    Task<TodoTask?> GetTodoTaskByIdAsync(int id, int listId);

    Task<TodoTask?> CreateTodoTaskAsync(TodoTask todo);

    Task<TodoTask?> UpdateTodoTaskAsync(TodoTask todo);

    Task<bool> DeleteTodoTaskAsync(int id, int listId);
}
