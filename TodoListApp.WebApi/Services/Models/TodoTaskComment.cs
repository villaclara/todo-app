namespace TodoListApp.WebApi.Services.Models;

public class TodoTaskComment
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public int TodoTaskId { get; set; }

    public TodoTask TodoTask { get; set; } = null!;

    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public DateTime DatePosted { get; set; }
}
