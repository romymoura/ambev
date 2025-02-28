using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class SetCacheCommandHandler<T> : IRequestHandler<SetCacheCommand<T>, CacheResult> where T : class
{
    private readonly IRedisCacheService _cacheService;
    public SetCacheCommandHandler(IRedisCacheService cacheService)
    {
        _cacheService = cacheService;
    }
    public async Task<CacheResult> Handle(SetCacheCommand<T> request, CancellationToken cancellationToken)
    {
        await _cacheService.SetAsync(request.Key, request.Value, request.Expiration);
        return new CacheResult { Action = true };
    }
}
