namespace TodoListApp.WebApp.Models;

public class BaseViewModel
{
    public Uri ReturnUrl { get; set; } = new Uri("/", UriKind.Relative);
}
