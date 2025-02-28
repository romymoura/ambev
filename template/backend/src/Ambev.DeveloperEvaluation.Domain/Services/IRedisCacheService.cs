namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface IRedisCacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T? value, TimeSpan expiration);
    Task UpdateAsync<T>(string key, T? value, TimeSpan expiration);
    Task DeleteAsync(string key);
}
