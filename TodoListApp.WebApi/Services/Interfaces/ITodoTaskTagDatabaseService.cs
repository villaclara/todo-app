using TodoListApp.Common.Models.TodoTaskTagModes;

namespace TodoListApp.WebApi.Services.Interfaces;

public interface ITodoTaskTagDatabaseService
{
    Task<List<TodoTaskTagModel>> GetAllTags();
}
