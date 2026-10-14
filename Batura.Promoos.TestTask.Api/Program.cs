using Batura.Promoos.TestTask.Api.Helpers;
using Batura.Promoos.TestTask.Api.Models;
using Batura.Promoos.TestTask.Api.Services;
using Batura.Promoos.TestTask.Model;
using Batura.Promoos.TestTask.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Batura.Promoos.TestTask.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            CompositionRoot.DefineDependencies(builder);

            var app = builder.Build();

            MapRoutes(app);

            app.Run();
        }

        private static void MapRoutes(WebApplication app)
        {
            app.MapPost(
                "/tasks", 
                async (
                    IDbContextFactory<TaskDbContext> contextFactory, 
                    [FromBody] CreateTaskApiRequest request, 
                    CancellationToken cancellationToken) =>
            {
                if (request == null)
                    return Results.BadRequest("Request cannot be null");

                var validationResult = ValidateRequest(request);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(v => v.ErrorMessage).ToList();

                    return Results.BadRequest(new { Errors = errors });
                }

                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                var taskId = Guid.NewGuid();

                context.TaskItems.Add(new TaskItem
                {
                    Id = taskId,
                    Title = request.Title,
                    Priority = TaskPriorityConverter.ConvertTo(request.Priority)
                });

                await context.SaveChangesAsync(cancellationToken);

                return Results.Ok(new { taskId });
            });

            app.MapGet(
                "/tasks", 
                async (
                    IDbContextFactory<TaskDbContext> contextFactory, 
                    CancellationToken cancellationToken) =>
            {
                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                var result = await context.TaskItems.ToListAsync(cancellationToken);

                return Results.Ok(result);
            });

            app.MapPut(
                "/tasks/{id}/complete", 
                async (
                    IDbContextFactory<TaskDbContext> contextFactory, 
                    [FromServices] ITaskCompletedPublisher serviceBus, 
                    [FromRoute] Guid id, 
                    CancellationToken cancellationToken) =>
            {
                if (id == Guid.Empty)
                    return Results.BadRequest("Invalid ID");

                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                var taskItem = await context.TaskItems.FindAsync([id], cancellationToken);

                if (taskItem is null)
                    return Results.NotFound("Task not found");

                if (taskItem.IsCompleted)
                    return Results.BadRequest("Task already completed");

                try
                {
                    taskItem.IsCompleted = true;
                    taskItem.CompletedAt = DateTimeOffset.UtcNow;

                    await context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Results.Conflict();
                }

                serviceBus.Publish(new ITaskCompletedPublisher.TaskCompletedMessage
                {
                    TaskId = taskItem.Id,
                    CompletedAt = DateTimeOffset.UtcNow,
                    Title = taskItem.Title,
                    Priority = TaskPriorityConverter.ConvertTo(taskItem.Priority)
                });

                return Results.Ok();
            });

            app.MapDelete(
                "/tasks/{id}", 
                async (
                    IDbContextFactory<TaskDbContext> contextFactory,
                    ILogger<Program> logger,
                    [FromRoute] Guid id,
                    CancellationToken cancellationToken) =>
            {
                if (id == Guid.Empty)
                    return Results.BadRequest("Invalid ID");

                using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

                var taskItem = await context.TaskItems.FindAsync([id], cancellationToken);

                if (taskItem is null)
                    return Results.NotFound();
                
                context.TaskItems.Remove(taskItem);

                await context.SaveChangesAsync(cancellationToken);

                logger.Log(LogLevel.Information, "Task was removed");

                return Results.Ok();
            });
        }

        private static ValidationInfo ValidateRequest(object request)
        {
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

            return new ValidationInfo(isValid, validationResults);
        }
    }

    record ValidationInfo(bool IsValid, List<ValidationResult> Errors);
}
