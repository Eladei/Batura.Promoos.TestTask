using RabbitMQ.Client;
using System.Text;

namespace Batura.Promoos.TestTask.Api.Services;

public sealed class RabbitMqService : IServiceBus
{
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqService> _logger;
    private readonly string _exchangeName;

    public RabbitMqService(IChannel channel, ILogger<RabbitMqService> logger, string exchangeName)
    {
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        if (string.IsNullOrWhiteSpace(exchangeName))
            throw new ArgumentNullException(nameof(exchangeName));

        _exchangeName = exchangeName;
    }

    public async Task PublishMessage(string message, string routingKey)
    {
        byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
        var props = new BasicProperties();

        try
        {
            await _channel.BasicPublishAsync(_exchangeName, routingKey, false, props, messageBodyBytes);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, $"Ошибка публикации сообщения exchangeName = '{_exchangeName}'; routingKey = '{routingKey}'");
        }
    }
}
