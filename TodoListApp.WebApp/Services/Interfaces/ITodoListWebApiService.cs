using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Services.Interfaces;

/// <summary>
/// Contracts for managing todo lists through Web API calls.
/// </summary>
public interface ITodoListWebApiService
{
    /// <summary>
    /// Retrieves all todo lists for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose todo lists are being retrieved.</param>
    /// <returns>A collection of todo lists. Returns empty collection if no lists are found.</returns>
    /// <remarks>This method uses the default pagination settings.</remarks>
    Task<IEnumerable<TodoList>> GetTodoListsAsync(int userId);

    /// <summary>
    /// Retrieves a paginated list of todo lists for a specific user.
    /// </summary>
    /// <param name="userId">The ID of the user whose todo lists are being retrieved.</param>
    /// <param name="page">The page number to retrieve (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <returns>A <see cref="PagedResults{TodoList}"/> containing the requested page of todo lists and pagination metadata.</returns>
    Task<PagedResults<TodoList>> GetPagedTodoListsAsync(int userId, int page, int pageSize);

    /// <summary>
    /// Retrieves a specific todo list by its ID for a given user.
    /// </summary>
    /// <param name="listId">The ID of the todo list to retrieve.</param>
    /// <param name="userId">The ID of the user who owns the todo list.</param>
    /// <returns>The requested todo list if found; otherwise, null.</returns>
    Task<TodoList?> GetTodoListByIdAsync(int listId, int userId);

    /// <summary>
    /// Creates a new todo list.
    /// </summary>
    /// <param name="list">The todo list to create.</param>
    /// <returns>The created todo list if successful; otherwise, null.</returns>
    /// <exception cref="ApplicationException"> Thrown when there is an error processing the request including exception message. </exception>
    Task<TodoList?> CreateTodoListAsync(TodoList list);

    /// <summary>
    /// Updates an existing todo list.
    /// </summary>
    /// <param name="list">The todo list with updated information.</param>
    /// <returns>The updated todo list if successful; otherwise, null.</returns>
    /// <exception cref="ApplicationException"> Thrown when there is an error processing the request including exception message. </exception>
    Task<TodoList?> UpdateTodoListAsync(TodoList list);

    /// <summary>
    /// Deletes a specific todo list.
    /// </summary>
    /// <param name="listId">The ID of the todo list to delete.</param>
    /// <param name="userId">The ID of the user who owns the todo list.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    /// <remarks>Returns false if the todo list was not found or if deletion failed.</remarks>
    Task<bool> DeleteTodoListAsync(int listId, int userId);
}
