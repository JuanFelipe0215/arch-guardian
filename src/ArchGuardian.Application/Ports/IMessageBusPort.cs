namespace ArchGuardian.Application.Ports;

public interface IMessageBusPort
{
    Task PublishAsync<T>(T message, CancellationToken ct = default);
}