using Ambev.DeveloperEvaluation.Domain.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Cache;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _database;
    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task DeleteAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var valueCache = await _database.StringGetAsync(key);
        if (valueCache.HasValue)
        {
            var result = JsonSerializer.Deserialize<T>(valueCache.ToString());
            return result;
        }
        return default;
    }

    public async Task SetAsync<T>(string key, T? value, TimeSpan expiration)
    {
        if (value is null)
            return;
        var serializedValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiration);
    }

    public async Task UpdateAsync<T>(string key, T? value, TimeSpan expiration)
    {
        if (value is null)
            return;
        var serializedValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiration);
    }
}
