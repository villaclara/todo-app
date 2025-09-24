using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Models;

namespace TodoListApp.WebApp.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.View();
    }

    public IActionResult Error()
    {
        return this.View("Error", new ErrorViewModel { RequestId = "Some internal error, sorry." });
    }
}
