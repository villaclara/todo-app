using System.ComponentModel.DataAnnotations;
using TodoListApp.Common.Models.Enums;

namespace TodoListApp.WebApp.Models;

public class TodoTaskViewModel : BaseViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is mandatory.")]
    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAtDate { get; set; }

    public DateTime DueToDate { get; set; }

    public TodoTaskStatus Status { get; set; }

    public int CreatedByUserId { get; set; }

    public string? CreatedByUserName { get; set; }

    public int AssigneeId { get; set; }

    [Required(ErrorMessage = "Assignee Name is mandatory")]
    public string AssigneeName { get; set; } = null!;

    public int TodoListId { get; set; }

    public string? TodoListName { get; set; }

    public bool IsOverdue => this.Status != TodoTaskStatus.Completed && this.DueToDate < DateTime.UtcNow;

    public List<TodoTaskTagViewModel> TagList { get; set; } = new List<TodoTaskTagViewModel>();

    public IEnumerable<TodoTaskCommentVIewModel>? CommentsList { get; set; }
}
