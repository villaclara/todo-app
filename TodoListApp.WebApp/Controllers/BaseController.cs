using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.WebApp.Areas.Identity.Data;

namespace TodoListApp.WebApp.Controllers;
public class BaseController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private ApplicationUser? cachedUser;

    protected BaseController(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    protected async Task<ApplicationUser> GetCurrentUserAsync()
    {
        this.cachedUser ??= await this.userManager.GetUserAsync(this.User);

        return this.cachedUser;
    }

    protected async Task<int> GetCurrentUserIdAsync()
    {
        var user = await this.GetCurrentUserAsync();
        return user.UserIntId;
    }
}
