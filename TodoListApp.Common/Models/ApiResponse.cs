namespace TodoListApp.Common.Models;

/// <summary>
/// Represents a standard API response wrapper.
/// </summary>
/// <typeparam name="T">The type of Data being returned in the response.</typeparam>
public class ApiResponse<T>
    where T : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
    /// </summary>
    public ApiResponse()
    {
        this.Data = new List<T>();
    }

    /// <summary>
    /// Gets or sets the data returned by API.
    /// </summary>
    public IEnumerable<T> Data { get; set; }
}
