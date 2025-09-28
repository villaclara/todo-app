using TodoListApp.Common.Models.Enums;

namespace TodoListApp.Common.Parameters.Filtering;
public class TodoTaskAssigneeFilter
{
    public DateTime? CreatedAfter { get; set; }
    public DateTime? CreatedBefore { get; set; }

    public DateTime? DueAfter { get; set; }
    public DateTime? DueBefore { get; set; }

    public TodoTaskStatus? Status { get; set; }

    public int? TodoListId { get; set; }
    public string? TodoListNameContains { get; set; }
}

