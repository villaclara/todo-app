using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoListApp.Common.Models.Enums;

namespace TodoListApp.WebApi.Entities;

/// <summary>
/// Represents a todo task entity in the database.
/// </summary>
public class TodoTaskEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the todo task.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the todo task.
    /// </summary>
    [MaxLength(100)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the todo task.
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the todo task was created.
    /// </summary>
    public DateTime CreatedAtDate { get; set; }

    /// <summary>
    /// Gets or sets the due date and time for the todo task.
    /// </summary>
    public DateTime DueToDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the todo task.
    /// </summary>
    public TodoTaskStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the id of user who created this task.
    /// </summary>
    public int CreatedByUserId { get; set; }

    /// <summary>
    /// Gets or sets the username who created this task.
    /// </summary>
    public string CreatedByUserName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier of the assignee for the task.
    /// </summary>
    public int AssigneeId { get; set; }

    /// <summary>
    /// Gets or sets the name of the assignee for the task.
    /// </summary>
    public string AssigneeName { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier of the todo list to which this task belongs.
    /// </summary>
    [ForeignKey(nameof(TodoListEntity))]
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the todo list entity that this task belongs to.
    /// </summary>
    public TodoListEntity TodoList { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of tags associated with this todo task.
    /// </summary>
    public ICollection<TodoTaskTagEntity> TagList { get; set; } = new List<TodoTaskTagEntity>();

    /// <summary>
    /// Gets or sets the collection of comments associated with this todo task.
    /// </summary>
    public ICollection<TodoTaskCommentEntity> CommentList { get; set; } = new List<TodoTaskCommentEntity>();
}
