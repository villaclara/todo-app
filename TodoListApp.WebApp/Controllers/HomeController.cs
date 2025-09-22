using Microsoft.AspNetCore.Mvc;

namespace TodoListApp.WebApp.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return this.View();
    }
}
