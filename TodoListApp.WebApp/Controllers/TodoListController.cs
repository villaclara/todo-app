using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

public class TodoListController : Controller
{
    private readonly ITodoListWebApiService listService;

    public TodoListController(ITodoListWebApiService service)
    {
        this.listService = service;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var todos = await this.listService.GetTodoLists(1);
        var result = todos.ToList();
        return this.View(result);
    }

    [HttpGet]
    [Route("/details/{listId}")]    // not mandatory
    public async Task<IActionResult> Details(int listId)
    {
        var todo = await this.listService.GetTodoListById(listId, 1);
        return this.View(todo);
    }
}
