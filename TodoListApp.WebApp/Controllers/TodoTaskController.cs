using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Interfaces;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Controllers;
public class TodoTaskController : Controller
{
    private readonly ITodoTaskWebApiService taskService;

    public TodoTaskController(ITodoTaskWebApiService taskService)
    {
        this.taskService = taskService;
    }

    public IActionResult Index()
    {
        return this.View();
    }

    [HttpGet]
    public async Task<IActionResult> CreateEdit(int id = 0, int listId = 0)
    {
        if (listId == 0)
        {
            return this.View("Error", new ErrorViewModel { RequestId = "Wrong Todo List Id specified.", ReturnUrl = new Uri($"/todolist/details?listId={id}", UriKind.Relative) });
        }

        if (id == 0)
        {
            return this.View(new TodoTaskViewModel()
            {
                // Setting Default values for the new TodoTask.
                // DueToDate will also be displayed in the Form in html file.
                TodoListName = "defaultName",
                DueToDate = DateTime.Now,
                TaskStatus = "NotStarted",
                TodoListId = listId,
            });
        }

        var todo = await this.taskService.GetTodoTaskByIdAsync(id, listId);   // TODO - id userId

        if (todo == null)
        {
            return this.View("Error", new ErrorViewModel { RequestId = $"Task with id {id} could not be found.", ReturnUrl = new Uri($"/todolist/details?listId={id}", UriKind.Relative) });
        }

        var model = new TodoTaskViewModel
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            Assignee = todo.Assignee,
            CreatedAtDate = todo.CreatedAtDate,
            DueToDate = todo.DueToDate,
            TaskStatus = todo.Status,
            TodoListId = todo.TodoListId,
            TodoListName = todo.TodoListName,
        };

        return this.View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEdit(TodoTaskViewModel model)
    {
        if (!this.ModelState.IsValid)
        {
            this.ModelState.AddModelError(" ", "Not Valid");
            return this.View("Error", new ErrorViewModel { RequestId = $"Validation Errors occurred.", ReturnUrl = new Uri($"/todolist/details?listId={model.TodoListId}", UriKind.Relative) });
        }

        try
        {
            TodoTask todo = new TodoTask
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueToDate = model.DueToDate,
                Assignee = model.Assignee ?? "bruh", // TODO - assignee
                TodoListId = model.TodoListId,
            };

            var act = model.Id switch
            {
                0 => this.taskService.CreateTodoTaskAsync(todo),
                not 0 => this.taskService.UpdateTodoTaskAsync(todo),
            };

            var result = await act;

            if (result == null)
            {
                return this.RedirectToAction(actionName: "Details", controllerName: "TodoList", new { listId = model.TodoListId });
            }

            return this.RedirectToAction(actionName: "Details", controllerName: "TodoList", new { listId = model.TodoListId });
        }
        catch (ApplicationException ex)
        {
            this.ViewBag.ErrorMessage = ex.Message;
            return this.View("Error", new ErrorViewModel { RequestId = ex.Message, ReturnUrl = new Uri("/todolist", UriKind.Relative) });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, int listId)
    {
        var todo = await this.taskService.GetTodoTaskByIdAsync(id, listId);  // TODO - id

        if (todo == null)
        {
            this.ViewBag.ErrorMessage = "No tasks found with this id.";
            return this.View("Error");
        }

        var result = await this.taskService.DeleteTodoTaskAsync(id, listId); // TODO - id

        if (!result)
        {
            this.ViewBag.ErrorMessage = "Error when deleting todotask.";
            return this.View("Error", new ErrorViewModel { RequestId = "Internal Error when deleting task", ReturnUrl = new Uri($"/todolist/details?listId={listId}", UriKind.Relative) });
        }

        return this.RedirectToAction(actionName: "Details", controllerName: "TodoList", new { listId });
    }
}
