using TodoListApp.Common.Models.TodoListModels;
using TodoListApp.Common.Models.TodoTaskCommentModels;
using TodoListApp.Common.Models.TodoTaskModels;
using TodoListApp.Common.Models.TodoTaskTagModes;
using TodoListApp.WebApi.Entities;
using TodoListApp.WebApi.Services.Models;

namespace TodoListApp.WebApi.Utility;

public static class WebApiMapper
{
    public static TOut MapTodoList<TIn, TOut>(TIn obj)
        where TIn : class
        where TOut : class
    {
        var result = obj switch
        {
            // entity -> domain
            TodoListEntity entity when typeof(TOut) == typeof(TodoList) => new TodoList
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                UserId = entity.UserId,
            } as TOut,

            // domain -> entity
            TodoList domain when typeof(TOut) == typeof(TodoListEntity) => new TodoListEntity
            {
                Id = domain.Id,
                Title = domain.Title,
                Description = domain.Description,
                UserId = domain.UserId,
            } as TOut,

            // domain -> api response
            TodoList domain when typeof(TOut) == typeof(TodoListModel) => new TodoListModel
            {
                Id = domain.Id,
                Title = domain.Title,
                Description = domain.Description,
                UserId = domain.UserId,
            } as TOut,

            // api response -> domain
            TodoListModel model when typeof(TOut) == typeof(TodoList) => new TodoList
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
            // entity -> domain
            TodoTaskEntity entity when typeof(TOut) == typeof(TodoTask) => new TodoTask
            {
                TodoListId = entity.TodoListId,
                Id = entity.Id,
                AssigneeName = entity.AssigneeName,
                AssigneeId = entity.AssigneeId,
                CreatedAtDate = entity.CreatedAtDate,
                DueToDate = entity.DueToDate,
                Description = entity.Description,
                Title = entity.Title,
                Status = entity.Status,
                TodoListName = entity.TodoList.Title,
                TagList = entity.TagList.Select(x => new TodoTaskTag { Id = x.Id, Title = x.Title }).ToList(),
            } as TOut,

            // domain -> entity
            TodoTask domain when typeof(TOut) == typeof(TodoTaskEntity) => new TodoTaskEntity
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
                TagList = domain.TagList.Select(x => new TodoTaskTagEntity { Id = x.Id, Title = x.Title }).ToList(),
            } as TOut,

            // domain -> api response
            TodoTask domain when typeof(TOut) == typeof(TodoTaskModel) => new TodoTaskModel
            {
                Id = domain.Id,
                AssigneeName = domain.AssigneeName,
                AssigneeId = domain.AssigneeId,
                CreatedAtDate = domain.CreatedAtDate,
                Description = domain.Description,
                DueToDate = domain.DueToDate,
                IsOverdue = domain.IsOverdue,
                Status = domain.Status,
                Title = domain.Title,
                TodoListName = domain.TodoListName,
                TodoListId = domain.TodoListId,
                TagList = domain.TagList.Select(x => new TodoTaskTagModel { Id = x.Id, Title = x.Title }).ToList(),
            } as TOut,

            // api response -> domain
            TodoTaskModel model when typeof(TOut) == typeof(TodoTask) => new TodoTask
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                DueToDate = model.DueToDate,
                Status = model.Status,
                AssigneeName = model.AssigneeName,
                AssigneeId = model.AssigneeId,
                TodoListId = model.TodoListId,
                CreatedAtDate = model.CreatedAtDate,
                TodoListName = model.TodoListName ?? string.Empty,
                TagList = model.TagList.Select(x => new TodoTaskTag { Id = x.Id, Title = x.Title }).ToList(),
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
            // entity -> domain
            TodoTaskCommentEntity entity when typeof(TOut) == typeof(TodoTaskComment) => new TodoTaskComment
            {
                Id = entity.Id,
                DatePosted = entity.DatePosted,
                Text = entity.Text,
                UserId = entity.UserId,
                UserName = entity.UserName,
                TodoTaskId = entity.TodoTaskId,
            } as TOut,

            // domain -> entity
            TodoTaskComment domain when typeof(TOut) == typeof(TodoTaskCommentEntity) => new TodoTaskCommentEntity
            {
                Id = domain.Id,
                DatePosted = domain.DatePosted,
                Text = domain.Text,
                UserId = domain.UserId,
                UserName = domain.UserName,
                TodoTaskId = domain.TodoTaskId,
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

            // api response -> domain
            TodoTaskCommentModel model when typeof(TOut) == typeof(TodoTaskComment) => new TodoTaskComment
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
