using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoListWebApiService
{
    Task<IEnumerable<TodoList>> GetTodoLists(int ownerId);

    Task<PagedResults<TodoList>> GetPagedTodoLists(int userId, int page, int pageSize);

    Task<TodoList> GetTodoListById(int listId, int userId);

    Task<TodoList?> CreateTodoList(TodoList list);

    Task<TodoList?> UpdateTodoList(TodoList list);

    Task<bool> DeleteTodoList(int listId, int userId);
}
