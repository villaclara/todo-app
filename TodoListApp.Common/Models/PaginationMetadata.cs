namespace TodoListApp.Common.Models;

public class PaginationMetadata
{
    public int CurrentPage { get; init; }
    public int TotalPages { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public bool HasPrevious => this.CurrentPage > 1;
    public bool HasNext => this.CurrentPage < this.TotalPages;

    public PaginationMetadata(int totalCount, int pageSize, int currentPage)
    {
        this.TotalCount = totalCount;
        this.PageSize = pageSize;
        this.CurrentPage = currentPage;
        this.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
