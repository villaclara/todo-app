namespace TodoListApp.Common.Parameters.Sorting;
public enum TaskSortingOptions
{
    CreatedDateDesc = 0, // default - newest on top
    CreatedDateAsc = 1,
    TodoListNameAsc = 2,
    TodoListNameDesc = 3,
    DueDateAsc = 4,
    DueDateDesc = 5,
    TaskStatusAsc = 6,
    TaskStatusDesc = 7,
}
