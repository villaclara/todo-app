using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListApp.WebApi.Entities;

/// <summary>
/// Represents a comment that is associated with one todo task.
/// </summary>
public class TodoTaskCommentEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Text value of comment.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for associated todo task.
    /// </summary>
    [ForeignKey(nameof(TodoTaskEntity))]
    public int TodoTaskId { get; set; }

    /// <summary>
    /// Gets or sets navigation property of associated todo task.
    /// </summary>
    public TodoTaskEntity TodoTaskEntity { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier of user who created the comment.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of user who created the comment.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date the comment was created.
    /// </summary>
    public DateTime DatePosted { get; set; }
}
