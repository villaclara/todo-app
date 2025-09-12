using Microsoft.EntityFrameworkCore;
using TodoListApp.WebApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Adding DbContext
builder.Services.AddDbContext<TodoListDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.Run();
