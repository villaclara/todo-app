using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoTaskWebApiService
{
    Task<IEnumerable<TodoTask>> GetTodoTasksAsync(int listId);

    Task<PagedResults<TodoTask>> GetPagedTodoTasksAsync(int listId, int page, int pageSize);

    Task<TodoTask?> GetTodoTaskByIdAsync(int id, int listId);

    Task<TodoTask?> CreateTodoTaskAsync(TodoTask list);

    Task<TodoTask?> UpdateTodoTaskAsync(TodoTask list);

    Task<bool> DeleteTodoTaskAsync(int id, int listId);
}
