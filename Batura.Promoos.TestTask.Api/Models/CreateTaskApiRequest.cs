using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Batura.Promoos.TestTask.Api.Models;

public sealed class CreateTaskApiRequest
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Priority Priority { get; set; } = Priority.Medium;
}
