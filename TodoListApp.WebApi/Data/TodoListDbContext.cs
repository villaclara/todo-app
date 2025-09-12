using Microsoft.EntityFrameworkCore;

namespace TodoListApp.WebApi.Data;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }
}
