using TodoListApp.Common.Models;

namespace TodoListApp.WebApp.Models;

public class PagedResults<T>
    where T : class
{
    public IEnumerable<T> Data { get; set; } = new List<T>();

    public PaginationMetadata Pagination { get; set; } = new PaginationMetadata(0, 0, 0);
}
