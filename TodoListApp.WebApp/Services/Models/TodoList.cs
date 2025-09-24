namespace TodoListApp.WebApp.Services.Models;

/// <summary>
/// Represents a todo list belonging to a specific user.
/// </summary>
public class TodoList
{
    /// <summary>
    /// Gets or sets the unique identifier of the todo list.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the todo list.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the todo list.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the identifier of the user who owns the todo list.
    /// </summary>
    public int UserId { get; set; }
}
