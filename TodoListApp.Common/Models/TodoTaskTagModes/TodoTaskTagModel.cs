using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Common.Models.TodoTaskTagModes;

/// <summary>
/// Represents a tag associated with a todo task.
/// </summary>
public class TodoTaskTagModel
{
    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the tag.
    /// </summary>
    [Required(ErrorMessage = "Tag Title is mandatory.")]
    public string Title { get; set; } = string.Empty;
}
