using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Entities.Enums;

namespace TodoListApp.WebApi.Services.Models;

public class TodoTask
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateOnly CreatedAtDate { get; set; }

    public DateOnly DueToDate { get; set; }

    public TodoTaskStatus TaskStatus { get; set; }

    public string Assignee { get; set; } = null!;

    public int TodoListId { get; set; }

    public string TodoListName { get; set; } = null!;

    public TodoListEntity TodoList { get; set; } = null!;

    public bool IsOverdue => this.TaskStatus != TodoTaskStatus.Completed && this.DueToDate < DateOnly.FromDateTime(DateTime.UtcNow);
}
