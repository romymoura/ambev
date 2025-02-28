using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.Cache;

public class RedisExpirationBackgroundService : BackgroundService
{
    private readonly IRedisKeyExpirationSubscriber _subscriber;
    private readonly ILogger<RedisExpirationBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public RedisExpirationBackgroundService(
        IRedisKeyExpirationSubscriber subscriber,
        ILogger<RedisExpirationBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _subscriber = subscriber;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Serviço de monitoramento de expiração Redis iniciado");


        await _subscriber.SubscribeToKeyExpirationAsync(async (expiredKey) =>
        {
            // Usar um escopo para obter serviços necessários para processar a expiração
            using var scope = _serviceProvider.CreateScope();

            _logger.LogInformation("Processando chave expirada: {Key}", expiredKey);

            // Aqui você pode implementar sua lógica de processamento para a chave expirada
            // Por exemplo, você pode querer recarregar dados, enviar notificações, etc.
            if (expiredKey.StartsWith("usuario:"))
            {
                // Exemplo: extrair ID do usuário da chave
                var userId = expiredKey.Split(':')[1];


                // Você poderia obter um serviço para lidar com usuários
                // var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                // await userService.HandleCacheExpiredAsync(userId);
                _logger.LogInformation("Cache de usuário expirado para ID: {UserId}", userId);
            }
            else if (expiredKey.StartsWith("shopping"))
            {
                // TODO: Aqui você pode chamar outro serviço, atualizar o banco de dados, etc.
                //await cartService.HandleCacheExpiredAsync(expiredKey);
                _logger.LogInformation("Cache de intenção de compra expirado: {Key}", expiredKey);
            }
            else
            {
                _logger.LogInformation("Chave genérica expirada: {Key}", expiredKey);
            }
        });

        // Manter o serviço em execução até ser cancelado
        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // Operação cancelada normalmente durante o desligamento
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Parando serviço de monitoramento de expiração Redis");
        if (_subscriber.IsSubscribed)
        {
            await _subscriber.UnsubscribeAsync();
        }
        await base.StopAsync(cancellationToken);
    }
}