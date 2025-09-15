using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Services.Interfaces;

/// <summary>
/// Provides operations for managing TodoLists.
/// </summary>
public interface ITodoListDatabaseService
{
    Task<List<TodoList>> GetAllForUserAsync(int userId);

    Task<TodoList?> GetByIdAsync(int userId, int todoListId);

    Task<TodoList?> AddAsync(TodoList todo);
}
