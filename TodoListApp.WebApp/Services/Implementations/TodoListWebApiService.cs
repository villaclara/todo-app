using System.Net;
using TodoListApp.Common.Models;
using TodoListApp.Common.Models.TodoListModels;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Implementations;

public class TodoListWebApiService : ITodoListWebApiService
{
    private readonly HttpClient http;
    private readonly ILogger<TodoListWebApiService> logger;

    public TodoListWebApiService(HttpClient http, ILogger<TodoListWebApiService> logger)
    {
        this.http = http;
        this.logger = logger;
    }

    public async Task<TodoList?> CreateTodoList(TodoList list)
    {
        var todoModel = new CreateTodoListModel
        {
            Title = list.Title,
            Description = list.Description,
            UserId = list.UserId,
        };

        try
        {
            var response = await this.http.PostAsJsonAsync($"api/todolist", todoModel);
            _ = response.EnsureSuccessStatusCode();

            var added = await response.Content.ReadFromJsonAsync<TodoList>();
            return added;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning("Error sending request to create a Todolist {@todolist}, ex - {@ex}.", todoModel, ex.Message);
            return null;
        }
    }

    public async Task<bool> DeleteTodoList(int listId, int userId)
    {
        var response = await this.http.DeleteAsync($"api/todolist/{listId}?userId={userId}");

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            return false;
        }

        return true;
    }

    public async Task<PagedResults<TodoList>> GetPagedTodoLists(int userId, int page = 1, int pageSize = 1)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<Common.Models.TodoListModels.TodoListModel>>($"api/todolist?userId={userId}&pagenumber={page}&pagesize={pageSize}");
        if (request == null)
        {
            return new PagedResults<TodoList>();
        }

        var result = new PagedResults<TodoList>()
        {
            Data = request.Data.Select(x => new TodoList()
            {
                Id = x.Id,
                Description = x.Description,
                Title = x.Title,
                UserId = x.UserId,
            }),
            Pagination = request.Pagination ?? new PaginationMetadata(0, 0, 0),
        };

        return result;
    }

    public async Task<TodoList> GetTodoListById(int listId, int userId)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<Common.Models.TodoListModels.TodoListModel>>($"api/todolist/{listId}?userId={userId}");
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
        var request = await this.http.GetFromJsonAsync<ApiResponse<Common.Models.TodoListModels.TodoListModel>>($"api/todolist?userId={ownerId}");
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

    public async Task<TodoList?> UpdateTodoList(TodoList list)
    {
        var model = new TodoListApp.Common.Models.TodoListModels.TodoListModel()
        {
            Id = list.Id,
            Description = list.Description,
            Title = list.Title,
            UserId = list.UserId,
        };

        try
        {
            var response = await this.http.PutAsJsonAsync($"api/todolist/{list.Id}?userId={list.UserId}", model);
            _ = response.EnsureSuccessStatusCode();

            return list;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning("Error sending request to create a Todolist {@todolist}, ex - {@ex}.", model, ex.Message);
            return null;
        }
    }
}
