using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

public interface ITodoTaskTagWebApiService
{
    Task<List<TodoTaskTag>> GetAllTasksAsync();
}
