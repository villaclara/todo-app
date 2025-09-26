using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class TodoTaskViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is mandatory.")]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAtDate { get; set; }

    public DateTime DueToDate { get; set; }

    public string TaskStatus { get; set; } = null!;

    public string Assignee { get; set; } = null!;

    public int TodoListId { get; set; }

    public string? TodoListName { get; set; }

    public bool IsOverdue => this.TaskStatus.ToLower() != "completed" && this.DueToDate < DateTime.UtcNow;
}
