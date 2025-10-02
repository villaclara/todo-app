namespace TodoListApp.Common.Models.TodoTaskCommentModels;

public class TodoTaskCommentModel
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public int TodoTaskId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public DateTime DatePosted { get; set; }
}
