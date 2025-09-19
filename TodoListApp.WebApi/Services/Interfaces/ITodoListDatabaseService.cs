using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Services.Interfaces;

/// <summary>
/// Contracts for performing CRUD operations on TodoList objects.
/// </summary>
public interface ITodoListDatabaseService
{
    /// <summary>
    /// Asynchronously adds new TodoList object into database.
    /// </summary>
    /// <param name="todo">List model to add.</param>
    /// <returns>Task that represents async operation. Taks contains created object as <see cref="TodoList"/> if success.</returns>
    Task<TodoList> CreateAsync(TodoList todo);

    /// <summary>
    /// Asynchronously gets the list of TodoList object for specified user.
    /// </summary>
    /// <param name="userId">Id of user to retrieve the lists.</param>
    /// <returns>Task that represents async operation. Task contains the list of <see cref="TodoList"/> object. Might be empty.</returns>
    Task<List<TodoList>> GetAllForUserAsync(int userId);

    /// <summary>
    /// Asynchronously get the <see cref="TodoList"/> object for specific user. <see langword="null"/>.
    /// </summary>
    /// <param name="userId">Id of user to retrieve the todolist.</param>
    /// <param name="todoListId">Id of list to retrieve.</param>
    /// <returns>Task that represents async operation. Task contains the <see cref="TodoList"/> object if succes, <see langword="null"/> if fail.</returns>
    Task<TodoList?> GetByIdAsync(int userId, int todoListId);

    /// <summary>
    /// Asynchronously updates the Todolist entity.
    /// </summary>
    /// <param name="todo"><see cref="TodoList"/> object that contains updates values.</param>
    /// <returns>Task that represents async operation. Task contains updated <see cref="TodoList"/> object.</returns>
    /// <exception cref="KeyNotFoundException">If the todolist was not found.</exception>
    Task<TodoList> UpdateAsync(TodoList todo);

    /// <summary>
    /// Asynchronously deletes the Todolist entity.
    /// </summary>
    /// <param name="userId">Id of user the todolist belongs to.</param>
    /// <param name="todoListId">Id of wanted todolist to delete.</param>
    /// <returns>Task that represents async operation. Task returns <see langword="true"/> if deleted, <see langword="false"/> otherwise.</returns>
    Task<bool> DeleteByIdAsync(int userId, int todoListId);
}
