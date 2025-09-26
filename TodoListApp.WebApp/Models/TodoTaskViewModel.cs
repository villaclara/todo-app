using System.ComponentModel.DataAnnotations;
using TodoListApp.Common.Models.Enums;

namespace TodoListApp.WebApp.Models;

public class TodoTaskViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is mandatory.")]
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAtDate { get; set; }

    public DateTime DueToDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public int AssigneeId { get; set; }

    public string AssigneeName { get; set; } = null!;

    public int TodoListId { get; set; }

    public string? TodoListName { get; set; }

    public bool IsOverdue => this.Status != TodoTaskStatus.Completed && this.DueToDate < DateTime.UtcNow;
}
