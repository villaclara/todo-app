using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Entities;

namespace TodoListApp.WebApi.Data;

public class TodoListDbContext : DbContext
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoListEntity> TodoLists { get; set; } = null!;

    public DbSet<TodoTaskEntity> TodoTasks { get; set; } = null!;
}
