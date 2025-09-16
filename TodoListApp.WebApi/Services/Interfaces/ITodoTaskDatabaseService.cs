using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoTaskDatabaseService
{
    Task<TodoTask> CreateAsync(TodoTask todoTask);

    Task<List<TodoTask>> GetAllForTodoListAsync(int todoListId);

    Task<TodoTask> GetByIdAsync(int id);

    Task<TodoTask> UpdateAsync(TodoTask todoTask);

    Task<bool> DeleteAsync(int id);
}
