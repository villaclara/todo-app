using TodoListApp.Common.Models.TodoListModels;
using TodoListApp.Common.Models.TodoTaskCommentModels;
using TodoListApp.Common.Models.TodoTaskModels;
using TodoListApp.Common.Models.TodoTaskTagModes;
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
                AssigneeName = model.AssigneeName,
                AssigneeId = model.AssigneeId,
                CreatedAtDate = model.CreatedAtDate,
                DueToDate = model.DueToDate,
                Description = model.Description,
                Title = model.Title,
                Status = model.Status,
                TodoListName = model.TodoListName ?? string.Empty,
                TagList = model.TagList.Select(x => new TodoTaskTag { Id = x.Id, Title = x.Title }).ToList(),
            } as TOut,

            // domain -> api response
            TodoTask domain when typeof(TOut) == typeof(TodoTaskModel) => new TodoTaskModel
            {
                TodoListId = domain.TodoListId,
                Id = domain.Id,
                AssigneeName = domain.AssigneeName,
                AssigneeId = domain.AssigneeId,
                CreatedAtDate = domain.CreatedAtDate,
                DueToDate = domain.DueToDate,
                Description = domain.Description,
                Title = domain.Title,
                Status = domain.Status,
                TodoListName = domain.TodoListName ?? string.Empty,
                TagList = domain.TagList.Select(x => new TodoTaskTagModel { Id = x.Id, Title = x.Title }).ToList(),
            } as TOut,

            // domain -> viewmodel
            TodoTask domain when typeof(TOut) == typeof(TodoTaskViewModel) => new TodoTaskViewModel
            {
                Id = domain.Id,
                Title = domain.Title,
                Description = domain.Description,
                AssigneeName = domain.AssigneeName,
                AssigneeId = domain.AssigneeId,
                CreatedAtDate = domain.CreatedAtDate,
                DueToDate = domain.DueToDate,
                Status = domain.Status,
                TodoListId = domain.TodoListId,
                TodoListName = domain.TodoListName,
                TagList = domain.TagList.Select(x => new TodoTaskTagViewModel { Id = x.Id, Title = x.Title }).ToList(),
            } as TOut,

            // viewmodel - domain
            TodoTaskViewModel vm when typeof(TOut) == typeof(TodoTask) => new TodoTask
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                DueToDate = vm.DueToDate,
                Status = vm.Status,
                AssigneeName = vm.AssigneeName,
                AssigneeId = vm.AssigneeId,
                TodoListId = vm.TodoListId,
                CreatedAtDate = vm.CreatedAtDate,
                TodoListName = vm.TodoListName ?? string.Empty,
                TagList = vm.TagList.Select(x => new TodoTaskTag { Id = x.Id, Title = x.Title }).ToList(),
            } as TOut,

            _ => throw new InvalidOperationException("Invalid types passed.")
        };

        return result ?? throw new InvalidCastException("Invalid Cast. Result is null");
    }

    public static TOut MapTodoTaskComment<TIn, TOut>(TIn obj)
       where TIn : class
       where TOut : class
    {
        var result = obj switch
        {
            // api response -> domain
            TodoTaskCommentModel entity when typeof(TOut) == typeof(TodoTaskComment) => new TodoTaskComment
            {
                Id = entity.Id,
                DatePosted = entity.DatePosted,
                Text = entity.Text,
                UserId = entity.UserId,
                UserName = entity.UserName,
                TodoTaskId = entity.TodoTaskId,
            } as TOut,

            // domain -> api response
            TodoTaskComment domain when typeof(TOut) == typeof(TodoTaskCommentModel) => new TodoTaskCommentModel
            {
                Id = domain.Id,
                DatePosted = domain.DatePosted,
                Text = domain.Text,
                UserId = domain.UserId,
                UserName = domain.UserName,
                TodoTaskId = domain.TodoTaskId,
            } as TOut,

            // domain -> viewmodel
            TodoTaskComment domain when typeof(TOut) == typeof(TodoTaskCommentVIewModel) => new TodoTaskCommentVIewModel
            {
                Id = domain.Id,
                DatePosted = domain.DatePosted,
                Text = domain.Text,
                UserId = domain.UserId,
                UserName = domain.UserName,
                TodoTaskId = domain.TodoTaskId,
            } as TOut,

            // viewmodel -> domain
            TodoTaskCommentVIewModel model when typeof(TOut) == typeof(TodoTaskComment) => new TodoTaskComment
            {
                Id = model.Id,
                DatePosted = model.DatePosted,
                Text = model.Text,
                UserId = model.UserId,
                UserName = model.UserName,
                TodoTaskId = model.TodoTaskId,
            } as TOut,

            _ => throw new InvalidOperationException("Invalid types passed.")
        };

        return result ?? throw new InvalidCastException("Invalid Cast. Result is null");
    }
}
