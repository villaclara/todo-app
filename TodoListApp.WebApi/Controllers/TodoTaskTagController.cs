using Microsoft.AspNetCore.Mvc;
using TodoListApp.Common.Models.TodoTaskTagModes;
using TodoListApp.WebApi.Services.Interfaces;

namespace TodoListApp.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TodoTaskTagController : ControllerBase
{
    private readonly ITodoTaskTagDatabaseService tagService;
    private readonly ILogger<TodoTaskTagController> logger;

    public TodoTaskTagController(ITodoTaskTagDatabaseService tagService, ILogger<TodoTaskTagController> logger)
    {
        this.tagService = tagService;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTags()
    {
        var tags = await this.tagService.GetAllTags();

        var result = tags.Select(x => new TodoTaskTagModel
        {
            Id = x.Id,
            Title = x.Title,
        });

        if (!result.Any())
        {
            this.logger.LogWarning("Tags are empty. Consider cheking database configuration.");
        }

        return this.Ok(result);
    }
}
