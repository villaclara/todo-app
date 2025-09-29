namespace TodoListApp.WebApi.Entities;

/// <summary>
/// Represents a tag that can be associated with one or more todo tasks.
/// </summary>
public class TodoTaskTagEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the tag.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the title or name of the tag.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the collection of todo tasks associated with this tag.
    /// </summary>
    public ICollection<TodoTaskEntity> TodoTasks { get; set; } = new List<TodoTaskEntity>();
}
