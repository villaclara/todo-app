using TodoListApp.Common.Models;
using TodoListApp.Common.Models.TodoTaskModels;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;

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

    public Task<TodoTask?> CreateTodoTaskAsync(TodoTask list)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteTodoTaskAsync(int listId, int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResults<TodoTask>> GetPagedTodoTasksAsync(int listId, int page = 1, int pageSize = 2)
    {
        var request = await this.http.GetFromJsonAsync<ApiResponse<TodoTaskModel>>($"api/todotask?listId={listId}&pagenumber={page}&pagesize={pageSize}");
        if (request == null)
        {
            return new PagedResults<TodoTask>();
        }

        var result = new PagedResults<TodoTask>()
        {
            Data = request.Data.Select(x => new TodoTask()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                TodoListId = x.TodoListId,
                Assignee = x.Assignee,
                CreatedAtDate = x.CreatedAtDate,
                DueToDate = x.DueToDate,
                TaskStatus = x.Status!,
                TodoListName = x.TodoListName!,
            }),
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

        return request.Data.Select(x => new TodoTask()
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            TodoListId = x.TodoListId,
            Assignee = x.Assignee,
            CreatedAtDate = x.CreatedAtDate,
            DueToDate = x.DueToDate,
            TaskStatus = x.Status!,
            TodoListName = x.TodoListName!,
        }).FirstOrDefault();
    }

    public Task<IEnumerable<TodoTask>> GetTodoTasksAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<TodoTask?> UpdateTodoTaskAsync(TodoTask list)
    {
        throw new NotImplementedException();
    }
}
