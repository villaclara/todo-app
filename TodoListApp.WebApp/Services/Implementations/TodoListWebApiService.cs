using Shared.Models;
using Shared.Models.TodoListModels;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Implementations;

public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient http;

    public TodoListWebApiService(HttpClient http)
    {
        this.http = http;
    }

    public async Task<TodoList> GetTodoListById(int listId, int userId)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoListModel>>($"api/todolist/{listId}?userId={userId}");
        if (request == null)
        {
            return new TodoList();
        }

        return request.Data.Select(x => new TodoList()
        {
            Id = x.Id,
            Description = x.Description,
            Title = x.Title,
            UserId = x.UserId,
        }).First();
    }

    public async Task<IEnumerable<TodoList>> GetTodoLists(int ownerId)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoListModel>>($"api/todolist?userId={ownerId}");
        if (request == null)
        {
            return new List<TodoList>();
        }

        return request.Data.Select(x => new TodoList()
        {
            Id = x.Id,
            Description = x.Description,
            Title = x.Title,
            UserId = x.UserId,
        });
    }
}
