using Batura.Promoos.TestTask.Api.BackgroundServices;
using Batura.Promoos.TestTask.Api.Options;
using Batura.Promoos.TestTask.Api.Services;
using Batura.Promoos.TestTask.Model;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace Batura.Promoos.TestTask.Api;

public static class CompositionRoot
{
    public static void DefineDependencies(WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddHostedService<EventBusStarter>();

        AddServiceBus(appBuilder);
        SetDbServices(appBuilder);
    }

    private static void AddServiceBus(WebApplicationBuilder appBuilder)
    {
        var services = appBuilder.Services;

        services.AddSingleton<IServiceBus>(provider =>
        {
            // TODO: Проводить валидацию конфигурации
            var rabbitMqConfig = appBuilder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqConfig.HostName,
                Port = rabbitMqConfig.Port,
                UserName = rabbitMqConfig.UserName,
                Password = rabbitMqConfig.Password,
                VirtualHost = rabbitMqConfig.VirtualHost,
                AutomaticRecoveryEnabled = rabbitMqConfig.AutomaticRecoveryEnabled,
                TopologyRecoveryEnabled = rabbitMqConfig.TopologyRecoveryEnabled,
                NetworkRecoveryInterval = TimeSpan.Parse(rabbitMqConfig.NetworkRecoveryInterval),
                ConsumerDispatchConcurrency = Convert.ToUInt16(rabbitMqConfig.ConsumerDispatchConcurrency),
                ClientProvidedName = rabbitMqConfig.ClientProvidedName
            };

            var connectionTask = factory.CreateConnectionAsync();
            var connection = connectionTask.GetAwaiter().GetResult();

            var channelTask = connection.CreateChannelAsync();
            var channel = channelTask.GetAwaiter().GetResult();

            var exchangeName = "task.events";
            var queueName = "task.events.queue";
            var routingKey = "task.completed";

            channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct).GetAwaiter().GetResult();
            channel.QueueDeclareAsync(queueName, true, false, false, null).GetAwaiter().GetResult();
            channel.QueueBindAsync(queueName, exchangeName, routingKey, null).GetAwaiter().GetResult();

            var logger = provider.GetRequiredService<ILogger<RabbitMqService>>();

            return new RabbitMqService(channel, logger, exchangeName);
        });

        services.AddSingleton<ITaskCompletedPublisher, TaskCompletedPublisher>();
    }

    private static void SetDbServices(WebApplicationBuilder appBuilder)
    {
        var connectionStr = appBuilder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentException("DefaultConnection not defined");

        appBuilder.Services.AddDbContextFactory<TaskDbContext>(
            options => options.UseNpgsql(connectionStr));
    }
}
