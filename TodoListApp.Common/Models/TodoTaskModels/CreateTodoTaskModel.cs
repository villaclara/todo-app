using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Common.Models.TodoTaskModels;

public class CreateTodoTaskModel
{
    [Required(ErrorMessage = "Title is mandatory.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Description is mandatory.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Due to Date is mandatory.")]
    public DateTime DueToDate { get; set; }

    public string? Assignee { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater or equal to 1.")]
    public int TodoListId { get; set; }
}
