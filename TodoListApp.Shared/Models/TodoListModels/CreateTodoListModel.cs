using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Shared.Models.TodoListModels;

/// <summary>
/// Data Transfer Object for creating new TodoList.
/// Used to transfer data between from request to service layer.
/// </summary>
public class CreateTodoListModel
{
    /// <summary>
    /// Gets or sets the Title of TodoList entity.
    /// </summary>
    [Required(ErrorMessage = "TodoList Title is mandatory.")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Description of the TodoList entity.
    /// </summary>
    [Required(ErrorMessage = "TodoList Description is mandatory.")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the User Id of TodoList entity.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater or equal to 1.")]
    public int UserId { get; set; }
}
