using System.ComponentModel.DataAnnotations;

namespace Batura.Promoos.TestTask.Model.Entities;

public sealed class TaskItem : EntityBase
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public TaskPriority Priority { get; set; }
}

public enum TaskPriority
{
    Low, 
    Medium, 
    High
}
