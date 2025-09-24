namespace TodoListApp.WebApp.Models;

/// <summary>
/// View model for displaying a paginated list of todo lists and pagination metadata.
/// </summary>
public class TodoListIndexViewModel
{
    /// <summary>
    /// Gets or sets the collection of todo lists to display on the current page.
    /// </summary>
    public List<TodoListViewModel> TodoLists { get; set; } = new List<TodoListViewModel>();

    /// <summary>
    /// Gets or sets the current page number (1-based).
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages available.
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Gets or sets the number of items displayed per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of todo lists available.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether there is a previous page available.
    /// </summary>
    public bool HasPrevious { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether there is a next page available.
    /// </summary>
    public bool HasNext { get; set; }
}
