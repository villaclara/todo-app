using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class TodoTaskModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is mandatory.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description is mandatory.")]
    public string Description { get; set; } = null!;

    public DateTime CreatedAtDate { get; set; }

    [Required]
    public DateTime DueToDate { get; set; }

    public string? Status { get; set; } = null!;

    [Required(ErrorMessage = "Assignee is mandatory.")]
    public string Assignee { get; set; } = null!;

    public string? TodoListName { get; set; }

    public int TodoListId { get; set; }

    public bool IsOverdue { get; set; }
}
