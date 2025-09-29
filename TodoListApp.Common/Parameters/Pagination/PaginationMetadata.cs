namespace TodoListApp.Common.Parameters.Pagination;

/// <summary>
/// Contains metadata about the pagination state of a result set.
/// </summary>
public class PaginationMetadata
{
    /// <summary>
    /// Gets the current page number (1-based).
    /// </summary>
    public int CurrentPage { get; init; }

    /// <summary>
    /// Gets the total number of pages available.
    /// </summary>
    public int TotalPages { get; init; }

    /// <summary>
    /// Gets the number of items per page.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Gets the total number of items in the result set.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Gets a value indicating whether there is a previous page.
    /// </summary>
    public bool HasPrevious => this.CurrentPage > 1;

    /// <summary>
    /// Gets a value indicating whether there is a next page.
    /// </summary>
    public bool HasNext => this.CurrentPage < this.TotalPages;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationMetadata"/> class.
    /// </summary>
    /// <param name="totalCount">Total number of items.</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="currentPage">Current page number.</param>
    public PaginationMetadata(int totalCount, int pageSize, int currentPage)
    {
        this.TotalCount = totalCount;
        this.PageSize = pageSize;
        this.CurrentPage = currentPage;
        this.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
