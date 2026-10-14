namespace Batura.Promoos.TestTask.Api.Services;

// TODO: Заготовка для mock-шины, функционирующей до инициализации реальной шины
public sealed class RabbitMqServiceMock : IServiceBus
{
    private readonly ILogger<RabbitMqServiceMock> _logger;

    public RabbitMqServiceMock(ILogger<RabbitMqServiceMock> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PublishMessage(string message, string routingKey)
    {
        _logger.LogInformation($"Шина сообщений еще не инициализирована. Сообщение не отправлено = '{message}'; routingKey = '{routingKey}'");
    }
}
