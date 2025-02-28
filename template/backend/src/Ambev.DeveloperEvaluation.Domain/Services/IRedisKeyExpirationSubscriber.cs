namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface IRedisKeyExpirationSubscriber
{
    Task SubscribeToKeyExpirationAsync(Func<string, Task> handler);
    Task UnsubscribeAsync();
    bool IsSubscribed { get; }
}
