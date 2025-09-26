using TodoListApp.Common.Models.TodoListModels;
using TodoListApp.Common.Models.TodoTaskModels;
using TodoListApp.WebApp.Models;
using TodoListApp.WebApp.Services.Models;

namespace TodoListApp.WebApp.Utility;

public static class WebAppMapper
{
    public static TOut MapTodoList<TIn, TOut>(TIn obj)
        where TIn : class
        where TOut : class
    {
        var result = obj switch
        {
            // api response -> domain
            TodoListModel model when typeof(TOut) == typeof(TodoList) => new TodoList
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
            } as TOut,

            // domain -> api response
            TodoList domain when typeof(TOut) == typeof(TodoListModel) => new TodoListModel
            {
                Id = domain.Id,
                Title = domain.Title,
                Description = domain.Description,
                UserId = domain.UserId,
            } as TOut,

            // domain -> viewmodel
            TodoList domain when typeof(TOut) == typeof(TodoListViewModel) => new TodoListViewModel
            {
                Id = domain.Id,
                Title = domain.Title,
                Description = domain.Description,
                UserId = domain.UserId,
            } as TOut,

            // viewmodel - domain
            TodoListViewModel model when typeof(TOut) == typeof(TodoList) => new TodoList
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
            } as TOut,

            _ => throw new InvalidOperationException("Invalid types passed.")

        };

        return result ?? throw new InvalidCastException("Invalid Cast. result is null");
    }

    public static TOut MapTodoTask<TIn, TOut>(TIn obj)
        where TIn : class
        where TOut : class
    {
        var result = obj switch
        {
            // api response -> domain
            TodoTaskModel model when typeof(TOut) == typeof(TodoTask) => new TodoTask
            {
                TodoListId = model.TodoListId,
                Id = model.Id,
                Assignee = model.Assignee,
                CreatedAtDate = model.CreatedAtDate,
                DueToDate = model.DueToDate,
                Description = model.Description,
                Title = model.Title,
                Status = model.Status,
                TodoListName = model.TodoListName ?? string.Empty,
            } as TOut,

            // domain -> api response
            TodoTask domain when typeof(TOut) == typeof(TodoTaskModel) => new TodoTaskModel
            {
                TodoListId = domain.TodoListId,
                Id = domain.Id,
                Assignee = domain.Assignee,
                CreatedAtDate = domain.CreatedAtDate,
                DueToDate = domain.DueToDate,
                Description = domain.Description,
                Title = domain.Title,
                Status = domain.Status,
                TodoListName = domain.TodoListName ?? string.Empty,
            } as TOut,

            // domain -> viewmodel
            TodoTask domain when typeof(TOut) == typeof(TodoTaskViewModel) => new TodoTaskViewModel
            {
                Id = domain.Id,
                Title = domain.Title,
                Description = domain.Description,
                Assignee = domain.Assignee,
                CreatedAtDate = domain.CreatedAtDate,
                DueToDate = domain.DueToDate,
                Status = domain.Status,
                TodoListId = domain.TodoListId,
                TodoListName = domain.TodoListName,
            } as TOut,

            // viewmodel - domain
            TodoTaskViewModel vm when typeof(TOut) == typeof(TodoTask) => new TodoTask
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                DueToDate = vm.DueToDate,
                Status = vm.Status,
                Assignee = vm.Assignee,
                TodoListId = vm.TodoListId,
                CreatedAtDate = vm.CreatedAtDate,
                TodoListName = vm.TodoListName ?? string.Empty,
            } as TOut,

            _ => throw new InvalidOperationException("Invalid types passed.")
        };

        return result ?? throw new InvalidCastException("Invalid Cast. Result is null");
    }
}
