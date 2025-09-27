using TodoListApp.Common.Models.Pagination;

namespace TodoListApp.WebApp.Models;

/// <summary>
/// Represents a paginated result set, including the data items and pagination metadata.
/// </summary>
/// <typeparam name="T">The type of the data items in the result set.</typeparam>
public class PagedResults<T>
    where T : class
{
    /// <summary>
    /// Gets or sets the collection of data items for the current page.
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets the pagination metadata, such as current page, total pages, and total item count.
    /// </summary>
    /// <remarks> Defaults the all values to 0. </remarks>
    public PaginationMetadata Pagination { get; set; } = new PaginationMetadata(0, 0, 0);
}
