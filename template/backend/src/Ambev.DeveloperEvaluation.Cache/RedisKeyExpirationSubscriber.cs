using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.Cache;

public class RedisKeyExpirationSubscriber : IRedisKeyExpirationSubscriber, IDisposable
{
    private readonly IConnectionMultiplexer _redis;
    private readonly ISubscriber _subscriber;
    private readonly ILogger<RedisKeyExpirationSubscriber> _logger;
    private readonly RedisSettings _redisSettings;
    private readonly IServer _server;
    private bool _disposed = false;
    private bool _isSubscribed = false;

    public bool IsSubscribed => _isSubscribed;

    public RedisKeyExpirationSubscriber(IOptions<RedisSettings> redisOptions, IConnectionMultiplexer redis, ILogger<RedisKeyExpirationSubscriber> logger)
    {
        _logger = logger;
        _redisSettings = redisOptions.Value;
        _redis = redis;
        _subscriber = _redis.GetSubscriber();

        // Verificar se o Redis está configurado para eventos de expiração
        _server = _redis.GetServer(_redis.GetEndPoints()[0]);
        var config = _server.ConfigGet(_redisSettings?.NotifykeyspaceEvents ?? string.Empty);

        if (!VerifyNotifykeyspaceEvents(_redisSettings.NotifykeyspaceEventsValue))
        {
            _logger.LogWarning("Revalidando configuração");
            if (VerifyNotifykeyspaceEvents(_redisSettings.NotifykeyspaceEventsValue))
                _logger.LogWarning("Tentativa de configuração com sucesso!");
            else
                _logger.LogWarning("Não foi possível configurar o serviço!");
        }
    }

    private bool VerifyNotifykeyspaceEvents(string value)
    {
        try
        {
            var config = _server.ConfigGet(_redisSettings?.NotifykeyspaceEvents ?? string.Empty);
            if (config.Length == 0 || !config[0].Value.Contains(value))
            {
                _logger.LogWarning("Redis não está configurado para eventos de expiração, vamos tentar configurar de acordo com o settings da aplicação" +
                    $"que é Server {_redisSettings.ConnectionString}, Chave= {_redisSettings.NotifykeyspaceEvents} Valor= {_redisSettings.NotifykeyspaceEventsValue}. " +
                    $"Se não der certo execute diretamente no Redis: CONFIG SET notify-keyspace-events Ex");
                _server.ConfigSet(_redisSettings?.NotifykeyspaceEvents, value);
            }
            return true;
        }
        catch 
        {
            _logger.LogWarning("Erro ao tentar configurar serviço");
        }
        return false;
    }

    public async Task SubscribeToKeyExpirationAsync(Func<string, Task> handler)
    {
        if (_isSubscribed)
            return;

        // Assinar o canal de eventos de expiração (canal padrão do Redis para eventos de expiração)
        await _subscriber.SubscribeAsync("__keyevent@0__:expired", async (channel, key) =>
        {
            try
            {
                _logger.LogInformation("Chave expirada detectada: {Key}", key);
                await handler(key.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar expiração da chave: {Key}", key);
            }
        });

        _isSubscribed = true;
        _logger.LogInformation("Inscrição em eventos de expiração de chaves Redis ativada");
    }

    public async Task UnsubscribeAsync()
    {
        if (!_isSubscribed)
            return;

        await _subscriber.UnsubscribeAsync("__keyevent@0__:expired");
        _isSubscribed = false;
        _logger.LogInformation("Inscrição em eventos de expiração de chaves Redis desativada");
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        if (disposing)
        {
            _redis?.Dispose();
        }
        _disposed = true;
    }
}
