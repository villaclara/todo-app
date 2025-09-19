using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models.TodoTaskModels;

/// <summary>
/// Data Transfer Object for Todo Task.
/// Used to transfer data between API and service layers.
/// </summary>
public class UpdateTodoTaskModel
{
    /// <summary>
    /// Gets or sets the Title.
    /// </summary>
    [Required(ErrorMessage = "Title is mandatory.")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the Due to Date for Todo Task.
    /// The Time values are 00.00.000Z.
    /// </summary>
    public DateTime? DueToDate { get; set; }

    /// <summary>
    /// Gets or sets the Status of Todo Task from <see cref="Entities.Enums.TodoTaskStatus"/>.
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// Gets or sets the Task Assignee.
    /// </summary>
    public string? Assignee { get; set; } = null!;
}
