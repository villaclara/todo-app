using TodoListApp.Common.Models.Enums;

namespace TodoListApp.Common.Models.TodoTaskModels;

/// <summary>
/// Data Transfer Object for Todo Task.
/// Used to transfer data between API Update request and service layers.
/// </summary>
public class UpdateTodoTaskModel
{
    /// <summary>
    /// Gets or sets the Title.
    /// </summary>
    public string? Title { get; set; } = null!;

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
    /// Gets or sets the Status of Todo Task from <see cref="TodoTaskStatus"/>.
    /// </summary>
    public TodoTaskStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the Id of Assigned user.
    /// </summary>
    public int AssigneeId { get; set; }

    /// <summary>
    /// Gets or sets the Task Assignee.
    /// </summary>
    public string? AssigneeName { get; set; } = null!;
}
