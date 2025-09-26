using System.Net;
using TodoListApp.Common.Models;
using TodoListApp.Common.Models.TodoTaskModels;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;
using TodoListApp.WebApp.Utility;

namespace TodoListApp.WebApp.Services.Implementations;

public class TodoTaskWebApiService : ITodoTaskWebApiService
{
    private readonly HttpClient http;
    private readonly ILogger<TodoTaskWebApiService> logger;

    public TodoTaskWebApiService(HttpClient http, ILogger<TodoTaskWebApiService> logger)
    {
        this.http = http;
        this.logger = logger;
    }

    public async Task<TodoTask?> CreateTodoTaskAsync(TodoTask todo)
    {
        var model = new CreateTodoTaskModel
        {
            Title = todo.Title,
            Description = todo.Description,
            DueToDate = todo.DueToDate,
            AssigneeName = todo.AssigneeName ?? "user", // TODO - Change user
            AssigneeId = todo.AssigneeId,
            TodoListId = todo.TodoListId,
        };

        try
        {
            var response = await this.http.PostAsJsonAsync($"api/todotask", model);
            _ = response.EnsureSuccessStatusCode();

            var added = await response.Content.ReadFromJsonAsync<TodoTask>();
            return added;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning("Error sending request to create a TodoTask {@todolist}, ex - {@ex}.", model, ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
        catch (Exception ex)
        {
            this.logger.LogWarning("Unknow exception, ex - {@ex}", ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
    }

    public async Task<bool> DeleteTodoTaskAsync(int id, int listId)
    {
        var response = await this.http.DeleteAsync($"api/todotask/{id}?listId={listId}");

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            return false;
        }

        return true;
    }

    public async Task<PagedResults<TodoTask>> GetPagedTodoTasksByListAsync(int listId, int page = 1, int pageSize = 2)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoTaskModel>>($"api/todotask?listId={listId}&pagenumber={page}&pagesize={pageSize}");
        if (request == null)
        {
            return new PagedResults<TodoTask>();
        }

        var result = new PagedResults<TodoTask>()
        {
            Data = request.Data.Select(x => WebAppMapper.MapTodoTask<TodoTaskModel, TodoTask>(x)),
            Pagination = request.Pagination ?? new PaginationMetadata(0, 0, 0),
        };

        return result;
    }

    public async Task<PagedResults<TodoTask>> GetPagedTodoTasksByAssigneeAsync(int assigneeId, int page = 1, int pageSize = 2)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoTaskModel>>($"api/todotask?assigneeId={assigneeId}&pagenumber={page}&pagesize={pageSize}");
        if (request == null)
        {
            return new PagedResults<TodoTask>();
        }

        var result = new PagedResults<TodoTask>()
        {
            Data = request.Data.Select(x => WebAppMapper.MapTodoTask<TodoTaskModel, TodoTask>(x)),
            Pagination = request.Pagination ?? new PaginationMetadata(0, 0, 0),
        };

        return result;
    }

    public async Task<TodoTask?> GetTodoTaskByIdAsync(int id, int listId)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoTaskModel>>($"api/todotask/{id}?listId={listId}");
        if (request == null)
        {
            return null;
        }

        return request.Data.Select(x => WebAppMapper.MapTodoTask<TodoTaskModel, TodoTask>(x)).FirstOrDefault();
    }

    public async Task<IEnumerable<TodoTask>> GetTodoTasksAsync(int listId)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoTaskModel>>($"api/todotask?listId={listId}");
        if (request == null)
        {
            return new List<TodoTask>();
        }

        return request.Data.Select(x => WebAppMapper.MapTodoTask<TodoTaskModel, TodoTask>(x));
    }

    public async Task<TodoTask?> UpdateTodoTaskAsync(TodoTask todo)
    {
        var model = new TodoTaskModel
        {
            Id = todo.Id,
            Status = todo.Status,
            Title = todo.Title,
            TodoListId = todo.TodoListId,
            Description = todo.Description,
            DueToDate = todo.DueToDate,
            AssigneeName = todo.AssigneeName ?? "user", // TODO - Change user
            AssigneeId = todo.AssigneeId,
        };

        try
        {
            var response = await this.http.PutAsJsonAsync($"api/todotask/{todo.Id}?listId={todo.TodoListId}", model);
            _ = response.EnsureSuccessStatusCode();

            return todo;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning("Error sending request to update a TodoTask {@todolist}, ex - {@ex}.", model, ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
        catch (Exception ex)
        {
            this.logger.LogWarning("Unknow exception, ex - {@ex}", ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
    }
}
