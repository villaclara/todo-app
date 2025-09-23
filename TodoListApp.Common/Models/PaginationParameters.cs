namespace TodoListApp.Common.Models;

public class PaginationParameters
{
    private const int MaxPageSize = 50;
    private int pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => this.pageSize;
        set => this.pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
