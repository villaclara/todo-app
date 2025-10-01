using System.ComponentModel.DataAnnotations;

namespace TodoListApp.Common.Models.TodoTaskCommentModels;
public class CreateTodoTaskCommentModel
{
    [Required(ErrorMessage = "Text is mandatory.")]
    public string Text { get; set; } = null!;

    [Required(ErrorMessage = "Task Id is mandatory.")]
    public int TodoTaskId { get; set; }

    [Required(ErrorMessage = "User Id is mandatory.")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "User name is mandatory.")]
    public string UserName { get; set; } = string.Empty;
}
