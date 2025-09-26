using TodoListApp.Common.Models.Enums;

namespace TodoListApp.WebApp.Services.Models;

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAtDate { get; set; }

    public DateTime DueToDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public int AssigneeId { get; set; }

    public string AssigneeName { get; set; } = null!;

    public int TodoListId { get; set; }

    public string TodoListName { get; set; } = null!;

    public bool IsOverdue => this.Status != TodoTaskStatus.Completed && this.DueToDate < DateTime.UtcNow;
}
