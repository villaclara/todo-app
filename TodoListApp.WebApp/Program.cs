using TodoListApp.WebApp.Services.Implementations;
using TodoListApp.WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
