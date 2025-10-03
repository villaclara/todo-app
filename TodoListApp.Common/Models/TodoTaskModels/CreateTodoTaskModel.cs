using System.ComponentModel.DataAnnotations;
using TodoListApp.Common.Models.TodoTaskTagModes;

namespace TodoListApp.Common.Models.TodoTaskModels;

/// <summary>
/// Represents the data required to create a new todo task.
/// </summary>
public class CreateTodoTaskModel
{
    /// <summary>
    /// Gets or sets the title of the todo task.
    /// </summary>
    [Required(ErrorMessage = "Title is mandatory.")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description of the todo task.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the due date for the todo task.
    /// </summary>
    [Required(ErrorMessage = "Due to Date is mandatory.")]
    public DateTime DueToDate { get; set; }

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
    [Range(1, int.MaxValue, ErrorMessage = "The value must be greater or equal to 1.")]
    public int TodoListId { get; set; }

    /// <summary>
    /// Gets or sets the List of Tags for the Task.
    /// </summary>
    public List<TodoTaskTagModel> TagList { get; set; } = new List<TodoTaskTagModel>();
}
