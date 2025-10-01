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

    public DbSet<TodoTaskTagEntity> TodoTaskTags { get; set; } = null!;

    public DbSet<TodoTaskCommentEntity> TodoTaskComments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        _ = modelBuilder.Entity<TodoTaskTagEntity>()
            .ToTable("TodoTaskTags")
            .HasData(
            new TodoTaskTagEntity { Id = 1, Title = "Work" },
            new TodoTaskTagEntity { Id = 2, Title = "Personal" },
            new TodoTaskTagEntity { Id = 3, Title = "Urgent" },
            new TodoTaskTagEntity { Id = 4, Title = "Low Priority" },
            new TodoTaskTagEntity { Id = 5, Title = "Errands" },
            new TodoTaskTagEntity { Id = 6, Title = "Shopping" },
            new TodoTaskTagEntity { Id = 7, Title = "Health" },
            new TodoTaskTagEntity { Id = 8, Title = "Finance" },
            new TodoTaskTagEntity { Id = 9, Title = "Study" },
            new TodoTaskTagEntity { Id = 10, Title = "Travel" });
    }
}
