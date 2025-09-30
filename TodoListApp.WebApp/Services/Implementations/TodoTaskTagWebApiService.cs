using TodoListApp.Common.Models.TodoTaskTagModes;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Implementations;

public class TodoTaskTagWebApiService : ITodoTaskTagWebApiService
{
    private readonly HttpClient http;

    public TodoTaskTagWebApiService(HttpClient http)
    {
        this.http = http;
    }

    public async Task<List<TodoTaskTag>> GetAllTasksAsync()
    {
        var request = await this.http.GetFromJsonAsync<List<TodoTaskTagModel>>("api/todotasktag");

        return request.Select(x => new TodoTaskTag { Id = x.Id, Title = x.Title }).ToList();
    }
}
