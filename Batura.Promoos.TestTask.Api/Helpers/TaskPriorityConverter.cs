using Batura.Promoos.TestTask.Api.Models;
using Batura.Promoos.TestTask.Model.Entities;

namespace Batura.Promoos.TestTask.Api.Helpers;

public static class TaskPriorityConverter
{
    public static TaskPriority ConvertTo(Priority priority) =>
        priority switch
        {
            Priority.Low => TaskPriority.Low,
            Priority.Medium => TaskPriority.Medium,
            Priority.High => TaskPriority.High,
            _ => throw new ArgumentException($"Priority can not be converted")
        };

    public static Priority ConvertTo(TaskPriority priority) =>
        priority switch
        {
            TaskPriority.Low => Priority.Low,
            TaskPriority.Medium => Priority.Medium,
            TaskPriority.High => Priority.High,
            _ => throw new ArgumentException($"TaskPriority can not be converted")
        };
}
