using TodoListApp.Common.Models.Enums;

namespace TodoListApp.Common.Parameters.Filtering;

/// <summary>
/// Provides filtering options for querying todo tasks by assignee and other criteria.
/// </summary>
public class TodoTaskAssigneeFilter
{
    /// <summary>
    /// Gets or sets the lower bound for the task creation date.
    /// </summary>
    public DateTime? CreatedAfter { get; set; }

    /// <summary>
    /// Gets or sets the upper bound for the task creation date.
    /// </summary>
    public DateTime? CreatedBefore { get; set; }

    /// <summary>
    /// Gets or sets the lower bound for the task due date.
    /// </summary>
    public DateTime? DueAfter { get; set; }

    /// <summary>
    /// Gets or sets the upper bound for the task due date.
    /// </summary>
    public DateTime? DueBefore { get; set; }

    /// <summary>
    /// Gets or sets the status to filter tasks by.
    /// </summary>
    public TodoTaskStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the ID of the todo list to filter tasks by.
    /// </summary>
    public int? TodoListId { get; set; }

    /// <summary>
    /// Gets or sets a substring to match against todo list names.
    /// </summary>
    public string? TodoListNameContains { get; set; }
}

