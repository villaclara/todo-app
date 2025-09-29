using Microsoft.EntityFrameworkCore;
using TodoListApp.Common.Models.TodoTaskTagModes;
using TodoListApp.WebApi.Data;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Services.Implementations;

public class TodoTaskTagDatabaseService : ITodoTaskTagDatabaseService
{
    private readonly TodoListDbContext context;

    public TodoTaskTagDatabaseService(TodoListDbContext context)
    {
        this.context = context;
    }

    public async Task<List<TodoTaskTagModel>> GetAllTags() =>
        await this.context.TodoTaskTags
            .Select(x => new TodoTaskTagModel
            {
                Id = x.Id,
                Title = x.Title,
            }).ToListAsync();
}
