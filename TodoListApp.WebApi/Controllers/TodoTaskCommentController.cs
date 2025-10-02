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

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] TodoTaskCommentModel model)
    {
        // TODO - Not sure if this is correct. As I dont know if I want to expose the Id to TodoListModel.
        if (id != model.Id)
        {
            return this.BadRequest();
        }

        var comment = WebApiMapper.MapTodoTaskComment<TodoTaskCommentModel, TodoTaskComment>(model);

        try
        {
            var result = await this.commentService.UpdateAsync(comment);
            return this.NoContent();
        }
        catch (KeyNotFoundException knfEx)
        {
            this.logger.LogError("{@Method} - {@ex}.", nameof(this.UpdateComment), knfEx.Message);
            return this.NotFound();
        }
        catch (DbUpdateException dbUpdateEx)
        {
            this.logger.LogError("{@Method} - Exception - {@ex}.", nameof(this.UpdateComment), dbUpdateEx.Message);

            // e.g. SQL unique constraint violation
            return this.Conflict("Database update failed.");
        }
        catch (Exception ex)
        {
            this.logger.LogError("{@Method} - Exception thrown - {@ex}.", nameof(this.UpdateComment), ex.Message);
            throw;
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteComment(int id)
    {
        // TODO - It also looks not very good as the false is also called when the todolist does not belong to the user.
        var result = await this.commentService.DeleteByIdAsync(id);
        if (!result)
        {
            this.logger.LogWarning("{@Method} - Comment with {@id} not deleted due to not found.", nameof(this.DeleteComment), id);
            return this.NotFound();
        }

        return this.NoContent();
    }
}
