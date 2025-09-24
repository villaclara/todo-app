using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;
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
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 2)
    {
        // Call your service to get the ApiResponse<TodoListModel>
        //var apiResponse = await this.listService.GetTodoLists(1);

        var apiResponse = await this.listService.GetPagedTodoLists(1, pageNumber, pageSize);

        var viewModel = new TodoListIndexViewModel
        {
            TodoLists = apiResponse.Data.Select(x => new TodoListModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                UserId = x.UserId,
            }).ToList(),
            CurrentPage = apiResponse.Pagination?.CurrentPage ?? 1,
            TotalPages = apiResponse.Pagination?.TotalPages ?? 1,
            PageSize = apiResponse.Pagination?.PageSize ?? 10,
            TotalCount = apiResponse.Pagination?.TotalCount ?? 0,
            HasPrevious = apiResponse.Pagination?.HasPrevious ?? false,
            HasNext = apiResponse.Pagination?.HasNext ?? false,
        };

        return this.View(viewModel);
    }

    [HttpGet]
    [Route("/details/{listId}")]    // not mandatory
    public async Task<IActionResult> Details(int listId)
    {
        var todo = await this.listService.GetTodoListById(listId, 1);
        return this.View(todo);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEdit(int id = 0)
    {
        if (id == 0)
        {
            return this.View(new TodoListModel());
        }

        var todo = await this.listService.GetTodoListById(id, 1);   // userId

        var model = new TodoListModel
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            UserId = todo.UserId,
        };

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEdit(TodoListModel model)
    {
        if (!this.ModelState.IsValid)
        {
            this.ModelState.AddModelError(" ", "Not Valid");
            return this.View("CreateEdit", model);
        }

        if (model.Id == 0)
        {
            // Create


        }
        else
        {

            // Edit
        }

        return this.RedirectToAction("Index");
    }
}
