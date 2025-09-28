using TodoListApp.Common.Parameters.Filtering;
using TodoListApp.Common.Parameters.Sorting;

namespace TodoListApp.WebApp.Models;

public class TodoTaskIndexViewModel
{
    public List<TodoTaskViewModel> TodoTasks { get; set; } = new List<TodoTaskViewModel>();

    public TaskSortingOptions Sorting { get; set; }

    public TodoTaskAssigneeFilter Filter { get; set; }

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
