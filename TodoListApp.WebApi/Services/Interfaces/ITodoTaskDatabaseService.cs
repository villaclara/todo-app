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
    /// Asynchronously gets the list of TodoTask object for specified list.
    /// </summary>
    /// <param name="todoListId">List id to retrieve the tasks.</param>
    /// <param name="assignee">Name of user to retrieve the assigned tasks.</param>
    /// <returns>Task that represents async operation. Task contains the list of <see cref="TodoTask"/> object. Might be empty.</returns>
    Task<List<TodoTask>> GetAllTodoTasksWithParamsAsync(int? todoListId, string? assignee);

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
