using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Services.Implementations;

public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient http;

    public TodoListWebApiService(HttpClient http)
    {
        this.http = http;
    }


}
