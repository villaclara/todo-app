using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApp.Areas.Identity.Data;
using TodoListApp.WebApp.Services.EmailSender;
using TodoListApp.WebApp.Services.Implementations;
using TodoListApp.WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UsersDbConnection") ?? throw new InvalidOperationException("Connection string 'UsersDbConnection' not found.");

builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<UsersDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITodoListWebApiService, TodoListWebApiService>();
builder.Services.AddScoped<ITodoTaskWebApiService, TodoTaskWebApiService>();
builder.Services.AddScoped<ITodoTaskTagWebApiService, TodoTaskTagWebApiService>();
builder.Services.AddScoped<ITodoTaskCommentWebApiService, TodoTaskCommentWebApiService>();

builder.Services.AddHttpClient<ITodoListWebApiService, TodoListWebApiService>("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseAddress"]);
});

builder.Services.AddHttpClient<ITodoTaskWebApiService, TodoTaskWebApiService>("ApiClient1", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseAddress"]);
});

builder.Services.AddHttpClient<ITodoTaskTagWebApiService, TodoTaskTagWebApiService>("ApiClient2", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseAddress"]);
});

builder.Services.AddHttpClient<ITodoTaskCommentWebApiService, TodoTaskCommentWebApiService>("ApiClient3", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseAddress"]);
});

builder.Services.AddTransient<IEmailSender, StubEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
