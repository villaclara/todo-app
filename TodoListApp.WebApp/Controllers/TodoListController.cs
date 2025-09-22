using TodoListApp.WebApp.Services.Interfaces;

namespace TodoListApp.WebApp.Controllers;

public class TodoListController
{
    private readonly ITodoListWebApiService listService;

    public TodoListController(ITodoListWebApiService service)
    {
        this.listService = service;
    }
}
