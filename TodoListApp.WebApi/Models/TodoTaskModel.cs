using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class TodoTaskModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is mandatory.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description is mandatory.")]
    public string Description { get; set; } = null!;

    [Required]
    public DateOnly CreatedAtDate { get; set; }

    [Required]
    public DateOnly DueToDate { get; set; }

    [Required(ErrorMessage = "Task Status is mandatory.")]
    public string Status { get; set; } = null!;

    [Required(ErrorMessage = "Assignee is mandatory.")]
    public string Assignee { get; set; } = null!;

    [Required(ErrorMessage = "TodoList Name for this Task is mandatory.")]
    public string TodoListName { get; set; } = null!;

    [Required]
    public bool IsOverdue { get; set; }
}
