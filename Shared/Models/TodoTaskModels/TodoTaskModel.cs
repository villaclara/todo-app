namespace Shared.Models.TodoTaskModels;

/// <summary>
/// Data Transfer Object for Todo Task.
/// Used to transfer data between API and service layers.
/// </summary>
public class TodoTaskModel
{
    /// <summary>
    /// Gets or sets the Id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Title.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Description.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Created Date for Todo Task.
    /// </summary>
    public DateTime CreatedAtDate { get; set; }

    /// <summary>
    /// Gets or sets the Due to Date for Todo Task.
    /// The Time values are 00.00.000Z.
    /// </summary>
    public DateTime DueToDate { get; set; }

    /// <summary>
    /// Gets or sets the Status of Todo Task from <see cref="TodoTaskStatus"/>.
    /// </summary>
    public string? Status { get; set; } = null!;

    /// <summary>
    /// Gets or sets the Task Assignee.
    /// User who created a Tod Task is assigned by default.
    /// </summary>
    public string Assignee { get; set; } = null!;

    /// <summary>
    /// Gets or sets the TodoList Title the Todo Task belongs to.
    /// </summary>
    public string? TodoListName { get; set; }

    /// <summary>
    /// Gets or sets the TodoList Id the Todo Task belongs to.
    /// </summary>
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the Todo Task is Overdue.
    /// </summary>
    public bool IsOverdue { get; set; }
}
