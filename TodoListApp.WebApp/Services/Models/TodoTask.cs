namespace TodoListApp.WebApp.Services.Models;

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAtDate { get; set; }

    public DateTime DueToDate { get; set; }

    public string Status { get; set; } = null!;

    public string Assignee { get; set; } = null!;

    public int TodoListId { get; set; }

    public string TodoListName { get; set; } = null!;

    public bool IsOverdue => this.Status.ToLower() != "completed" && this.DueToDate < DateTime.UtcNow;
}
