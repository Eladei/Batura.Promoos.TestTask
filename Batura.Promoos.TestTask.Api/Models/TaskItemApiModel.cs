using System.ComponentModel.DataAnnotations;

namespace Batura.Promoos.TestTask.Api.Models;

public sealed class TaskItemApiModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public Priority Priority { get; set; }
}

public enum Priority
{
    Low,
    Medium,
    High
}
