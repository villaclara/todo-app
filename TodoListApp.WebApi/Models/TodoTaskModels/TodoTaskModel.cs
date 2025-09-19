namespace TodoListApp.WebApi.Models.TodoTaskModels;

public class TodoTaskModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAtDate { get; set; }

    public DateTime DueToDate { get; set; }

    public string? Status { get; set; } = null!;

    public string Assignee { get; set; } = null!;

    public string? TodoListName { get; set; }

    public int TodoListId { get; set; }

    public bool IsOverdue { get; set; }
}
