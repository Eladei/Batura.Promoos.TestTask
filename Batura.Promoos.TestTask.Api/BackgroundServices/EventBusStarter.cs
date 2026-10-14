using Batura.Promoos.TestTask.Api.Services;

namespace Batura.Promoos.TestTask.Api.BackgroundServices;

public class EventBusStarter : IHostedService
{
    private readonly IServiceBus _eventBus;

    public EventBusStarter(IServiceBus eventBus)
    {
        _eventBus = eventBus;
    }

    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task StopAsync(CancellationToken cancelToken) => Task.CompletedTask; // TODO: Реализовать корректное завершение работы
}
