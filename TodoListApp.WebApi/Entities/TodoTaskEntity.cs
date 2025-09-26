using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TodoListApp.Common.Models.Enums;

namespace TodoListApp.WebApi.Entities;

public class TodoTaskEntity
{
    [Key]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [MaxLength(200)]
    public string Description { get; set; } = null!;

    public DateTime CreatedAtDate { get; set; }

    public DateTime DueToDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public string Assignee { get; set; } = null!;

    [ForeignKey(nameof(TodoListEntity))]
    public int TodoListId { get; set; }

    public TodoListEntity TodoList { get; set; } = null!;
}
