namespace Batura.Promoos.TestTask.Api.Services;

public interface IServiceBus
{
    Task PublishMessage(string message, string routingKey);
}
