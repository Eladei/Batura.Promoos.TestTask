namespace Batura.Promoos.TestTask.Api.Options;

public sealed class RabbitMqOptions
{
    public string HostName { get; set; } = null!;
    public int Port { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string VirtualHost { get; set; } = null!;
    public bool AutomaticRecoveryEnabled { get; set; }
    public bool TopologyRecoveryEnabled { get; set; }
    public string NetworkRecoveryInterval { get; set; } = null!;
    public int ConsumerDispatchConcurrency { get; set; }
    public string ClientProvidedName { get; set; } = null!;
}
