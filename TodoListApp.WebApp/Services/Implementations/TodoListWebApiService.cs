using System.Net;
using TodoListApp.Common;
using TodoListApp.Common.Models.TodoListModels;
using TodoListApp.Common.Parameters.Pagination;
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

    public async Task<TodoList?> CreateTodoListAsync(TodoList list)
    {
        var model = new CreateTodoListModel
        {
            Title = list.Title,
            Description = list.Description,
            UserId = list.UserId,
        };

        try
        {
            var response = await this.http.PostAsJsonAsync($"api/todolist", model);
            _ = response.EnsureSuccessStatusCode();

            var added = await response.Content.ReadFromJsonAsync<TodoList>();
            return added;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning("Error sending request to create a Todolist {@todolist}, ex - {@ex}.", model, ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
        catch (Exception ex)
        {
            this.logger.LogWarning("Unknow exception, ex - {@ex}", ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
    }

    public async Task<bool> DeleteTodoListAsync(int listId, int userId)
    {
        var response = await this.http.DeleteAsync($"api/todolist/{listId}?userId={userId}");

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            return false;
        }

        return true;
    }

    public async Task<PagedResults<TodoList>> GetPagedTodoListsAsync(int userId, int page = 1, int pageSize = 1)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoListModel>>($"api/todolist?userId={userId}&pagenumber={page}&pagesize={pageSize}");
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

    public async Task<TodoList?> GetTodoListByIdAsync(int listId, int userId)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoListModel>>($"api/todolist/{listId}?userId={userId}");
        if (request == null)
        {
            return null;
        }

        return request.Data.Select(x => new TodoList()
        {
            Id = x.Id,
            Description = x.Description,
            Title = x.Title,
            UserId = x.UserId,
        }).FirstOrDefault();
    }

    public async Task<IEnumerable<TodoList>> GetTodoListsAsync(int userId)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoListModel>>($"api/todolist?userId={userId}");
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

    public async Task<TodoList?> UpdateTodoListAsync(TodoList list)
    {
        var model = new TodoListModel()
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
            this.logger.LogWarning("Error sending request to update a Todolist {@todolist}, ex - {@ex}.", model, ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
        catch (Exception ex)
        {
            this.logger.LogWarning("Unknow exception, ex - {@ex}", ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
    }
}
