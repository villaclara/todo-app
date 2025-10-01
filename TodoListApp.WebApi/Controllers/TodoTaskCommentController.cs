using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Common;
using TodoListApp.Common.Models.TodoListModels;
using TodoListApp.Common.Models.TodoTaskCommentModels;
using TodoListApp.WebApi.Services.Interfaces;
using TodoListApp.WebApi.Services.Models;
using TodoListApp.WebApi.Utility;

namespace TodoListApp.WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TodoTaskCommentController : ControllerBase
{
    private readonly ITodoTaskCommentDatabaseService commentService;
    private readonly ILogger<TodoTaskCommentController> logger;

    public TodoTaskCommentController(ITodoTaskCommentDatabaseService commentService, ILogger<TodoTaskCommentController> logger)
    {
        this.commentService = commentService;
        this.logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TodoListModel>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCommentsForTask(int taskId)
    {
        if (taskId == 0)
        {
            return this.BadRequest("Id of task can not be 0.");
        }

        var request = await this.commentService.GetCommentsForTaskByIdAsync(taskId);
        var result = request.Select(x => WebApiMapper.MapTodoTaskComment<TodoTaskComment, TodoTaskCommentModel>(x));

        return this.Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommentForTask(CreateTodoTaskCommentModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            var comment = new TodoTaskComment
            {
                Text = model.Text,
                TodoTaskId = model.TodoTaskId,
                DatePosted = DateTime.Now,
                UserId = model.UserId,
                UserName = model.UserName,
            };

            var result = await this.commentService.CreateAsync(comment);

            if (result == null)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error happened.");
            }

            var mapped = WebApiMapper.MapTodoTaskComment<TodoTaskComment, TodoTaskCommentModel>(result);

            return this.CreatedAtAction(nameof(this.GetCommentsForTask), mapped.TodoTaskId, mapped);
        }
        catch (DbUpdateException dbEx)
        {
            this.logger.LogError("{@Method} - Exception - {@ex}.", nameof(this.CreateCommentForTask), dbEx.Message);

            // e.g. SQL unique constraint violation
            return this.Conflict("Database update failed.");
        }
        catch (ArgumentException argEx)
        {
            return this.BadRequest(argEx.Message);
        }
        catch (Exception ex)
        {
            this.logger.LogError("{@Method} - Exception thrown - {@ex}.", nameof(this.CreateCommentForTask), ex.Message);
            throw;
        }
    }
}
