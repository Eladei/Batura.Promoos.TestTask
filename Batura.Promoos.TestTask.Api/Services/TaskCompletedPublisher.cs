using System.Text.Json;

namespace Batura.Promoos.TestTask.Api.Services;

public class TaskCompletedPublisher : ITaskCompletedPublisher
{
    private const string ROUTING_KEY = "task.completed";

    private readonly IServiceBus _serviceBus;

    public TaskCompletedPublisher(IServiceBus serviceBus)
    {
        _serviceBus = serviceBus 
            ?? throw new ArgumentNullException(nameof(serviceBus));
    }

    public void Publish(ITaskCompletedPublisher.TaskCompletedMessage message)
    {
        var messageStr = JsonSerializer.Serialize(message);

        _serviceBus.PublishMessage(messageStr, ROUTING_KEY);
    }
}
