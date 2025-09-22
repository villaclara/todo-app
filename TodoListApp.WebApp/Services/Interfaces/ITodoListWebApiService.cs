using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoListWebApiService
{
    Task<IEnumerable<TodoList>> GetTodoLists(int ownerId);

    Task<TodoList> GetTodoListById(int listId, int userId);
}
