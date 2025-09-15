using System.ComponentModel.DataAnnotations;

namespace TodoListApp.WebApi.Entities;

/// <summary>
/// Represents the list of Todos.
/// </summary>
public class TodoListEntity
{
    /// <summary>
    /// Gets or sets the Id of the TodoList entity.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Title of TodoList entity.
    /// </summary>
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Description of the TodoList entity.
    /// </summary>
    [MaxLength(200)]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the User Id of TodoList entity.
    /// </summary>
    public int UserId { get; set; }
}
