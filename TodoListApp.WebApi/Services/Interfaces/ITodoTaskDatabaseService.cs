using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Services.Interfaces;

/// <summary>
/// Contracts for performing CRUD operations on TodoTask objects.
/// </summary>
public interface ITodoTaskDatabaseService
{
    /// <summary>
    /// Asynchronously adds new TodoTask object into database. May return <see langword="null"/>.
    /// </summary>
    /// <param name="todoTask">Task model to add.</param>
    /// <returns>Task that represents async operation. Taks contains created object as <see cref="TodoTask"/> if success, <see langword="null"/> if fail.</returns>
    Task<TodoTask> CreateAsync(TodoTask todoTask);

    /// <summary>
    /// Asynchronously retrieves a paginated list of todo tasks based on specified filters.
    /// </summary>
    /// <param name="todoListId">The ID of the todo list to filter tasks. If null, returns tasks from all lists.</param>
    /// <param name="assignee">The name of the assignee to filter tasks. If null, returns tasks for all assignees.</param>
    /// <param name="page">The page number for pagination (1-based). If null, defaults to first page.</param>
    /// <param name="pageSize">The number of items per page. If null, uses default page size.</param>
    /// <returns>
    /// Task that represents async operation. Taks contains
    /// - totalCount: The total number of tasks matching the filter criteria
    /// - todoTasks: A list of <see cref="TodoTask"/> objects for the requested page.
    /// </returns>
    Task<(int totalCount, List<TodoTask> todoTasks)> GetAllTodoTasksWithParamsAsync(int? todoListId, int? assigneeId, int? page, int? pageSize);

    /// <summary>
    /// Asynchronously get the <see cref="TodoTask"/> object for specific user. <see langword="null"/>.
    /// </summary>
    /// <param name="id">Id of task to retrieve.</param>
    /// <returns>Task that represents async operation. Task contains the <see cref="TodoTask"/> object if succes, <see langword="null"/> if fail.</returns>
    Task<TodoTask?> GetByIdAsync(int id);

    /// <summary>
    /// Asynchronously updates the TodoTask entity.
    /// </summary>
    /// <param name="todoTask"><see cref="TodoTask"/> object that contains updates values.</param>
    /// <returns>Task that represents async operation. Task returns updated <see cref="TodoTask"/> object.</returns>
    /// <exception cref="KeyNotFoundException">If the task was not found.</exception>
    Task<TodoTask> UpdateAsync(TodoTask todoTask);

    /// <summary>
    /// Asynchronously deletes the TodoTask entity.
    /// </summary>
    /// <param name="id">Id of wanted todotask to delete.</param>
    /// <returns>Task that represents async operation. Task returns <see langword="true"/> if deleted, <see langword="false"/> otherwise.</returns>
    Task<bool> DeleteAsync(int id);
}
