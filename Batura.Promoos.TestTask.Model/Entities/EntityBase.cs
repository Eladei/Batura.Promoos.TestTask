using System.ComponentModel.DataAnnotations;

namespace Batura.Promoos.TestTask.Model.Entities;

public abstract class EntityBase
{
    public uint Version { get; set; }

    [Required]
    public DateTimeOffset CreatedAt { get; set; }
}
