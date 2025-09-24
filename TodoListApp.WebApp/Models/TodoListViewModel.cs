using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApp.Models;

public class TodoListViewModel
{
    /// <summary>
    /// Gets or sets the Id of the TodoList.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Title of TodoList.
    /// </summary>
    [Required(ErrorMessage = "TodoList Title is mandatory.")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Description of the TodoList.
    /// </summary>
    [Required(ErrorMessage = "TodoList Description is mandatory.")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the User Id of TodoList.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater or equal to 1.")]
    public int UserId { get; set; }
}
