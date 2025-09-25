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
        if (id == 0)
        {
            return this.View(new TodoTaskViewModel());
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
            TaskStatus = todo.TaskStatus,
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
            return this.View("CreateEdit", model);
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
                not 0 => this.taskService.UpdateTodoTaskAsync(todo),
                _ => this.taskService.CreateTodoTaskAsync(todo),
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
}
