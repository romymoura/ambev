using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class GetCacheCommandHandler<T> : IRequestHandler<GetCacheCommand<T>, CacheResult<T>> where T : class
{
    private readonly IRedisCacheService _cache;
    public GetCacheCommandHandler(IRedisCacheService cache)
    {
        _cache = cache;
    }
    public async Task<CacheResult<T>> Handle(GetCacheCommand<T> request, CancellationToken cancellationToken)
    {
        var result = await _cache.GetAsync<T>(request.Key);
        return new CacheResult<T> { Action = true, Value = result };
    }
}