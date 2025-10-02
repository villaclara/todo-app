using System.Net;
using TodoListApp.Common.Models.TodoTaskCommentModels;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;
using TodoListApp.WebApp.Utility;

namespace TodoListApp.WebApp.Services.Implementations;

public class TodoTaskCommentWebApiService : ITodoTaskCommentWebApiService
{
    private readonly HttpClient http;
    private readonly ILogger<TodoTaskCommentWebApiService> logger;

    public TodoTaskCommentWebApiService(HttpClient http, ILogger<TodoTaskCommentWebApiService> logger)
    {
        this.http = http;
        this.logger = logger;
    }

    public async Task<TodoTaskComment?> AddCommentAsync(TodoTaskComment comment)
    {
        var model = new CreateTodoTaskCommentModel
        {
            Text = comment.Text,
            UserId = comment.UserId,
            TodoTaskId = comment.TodoTaskId,
            UserName = comment.UserName,
        };

        try
        {
            var response = await this.http.PostAsJsonAsync($"api/todotaskcomment", model);
            _ = response.EnsureSuccessStatusCode();

            var added = await response.Content.ReadFromJsonAsync<TodoTaskComment>();
            return added;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning("Error sending request to create a comment {@todolist}, ex - {@ex}.", model, ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
        catch (Exception ex)
        {
            this.logger.LogWarning("Unknow exception, ex - {@ex}", ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
    }

    public async Task<IEnumerable<TodoTaskComment>> GetCommentsForTaskAsync(int taskId)
    {
        var request = await this.http.GetFromJsonAsync<IEnumerable<TodoTaskCommentModel>>($"api/todotaskcomment?taskId={taskId}");
        if (request == null)
        {
            return new List<TodoTaskComment>();
        }

        var result = request.Select(x => WebAppMapper.MapTodoTaskComment<TodoTaskCommentModel, TodoTaskComment>(x)).ToList();

        return result;
    }

    public async Task<TodoTaskComment> UpdateCommentAsync(TodoTaskComment comment)
    {
        var model = WebAppMapper.MapTodoTaskComment<TodoTaskComment, TodoTaskCommentModel>(comment);

        try
        {
            var response = await this.http.PutAsJsonAsync($"api/todotaskcomment/{comment.Id}", model);
            _ = response.EnsureSuccessStatusCode();

            return comment;
        }
        catch (HttpRequestException ex)
        {
            this.logger.LogWarning("Error sending request to update a Comment {@todolist}, ex - {@ex}.", model, ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
        catch (Exception ex)
        {
            this.logger.LogWarning("Unknow exception, ex - {@ex}", ex.Message);
            throw new ApplicationException($"Internal error when processing the request - {ex.Message}");
        }
    }

    public Task<bool> DeleteAllCommentsForTaskIdAsync(int taskId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteByIdAsync(int commentId)
    {
        var response = await this.http.DeleteAsync($"api/todotaskcomment/{commentId}");

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            return false;
        }

        return true;
    }
}
