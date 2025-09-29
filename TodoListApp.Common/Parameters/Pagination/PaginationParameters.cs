namespace TodoListApp.Common.Parameters.Pagination;

public class PaginationParameters
{
    private const int MaxPageSize = 50;
    private int pageSize = 5;

    /// <summary>
    /// Gets or sets the page number to retrieve (1-based).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize
    {
        get => this.pageSize;
        set => this.pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
