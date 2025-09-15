using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Models;

public class TodoListModel
{
    /// <summary>
    /// Gets or sets the Id of the TodoList entity.
    /// </summary>
    public int Id { get; set; }

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
    [Required(ErrorMessage = "User Id is mandatory.")]
    public int UserId { get; set; }
}
