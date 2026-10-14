using Batura.Promoos.TestTask.Api.Models;
using System.Text.Json.Serialization;

namespace Batura.Promoos.TestTask.Api.Services;

public interface ITaskCompletedPublisher
{
    void Publish(TaskCompletedMessage message);

    public record TaskCompletedMessage
    {
        public required Guid TaskId { get; init; }
        
        public required string Title { get; init; }

        public required DateTimeOffset CompletedAt { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priority Priority { get; init; }
    }
}
