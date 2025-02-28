using Ambev.DeveloperEvaluation.Cache;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        // Cache
        builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var redisSettings = configuration.GetSection("Redis").Get<RedisSettings>();

            var configOptions = ConfigurationOptions.Parse(redisSettings.ConnectionString);
            configOptions.Password = redisSettings.Password;
            configOptions.AllowAdmin = true;

            var redis = ConnectionMultiplexer.Connect(configOptions);
           

            return redis;
        });

        builder.Services.AddTransient<IRedisCacheService, RedisCacheService>();
        //builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

        // Notificação Cache
        builder.Services.AddSingleton<IRedisKeyExpirationSubscriber, RedisKeyExpirationSubscriber>();

        // Notificação Cache background
        builder.Services.AddHostedService<RedisExpirationBackgroundService>(); // Listener de expiração das chaves do Redis.
    }
}