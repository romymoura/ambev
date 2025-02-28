using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class UpdateCacheCommandHandler<T> : IRequestHandler<UpdateCacheCommand<T>, CacheResult> where T : class
{
    private readonly IRedisCacheService _cacheService;
    public UpdateCacheCommandHandler(IRedisCacheService cacheService)
    {
        _cacheService = cacheService;
    }
    public async Task<CacheResult> Handle(UpdateCacheCommand<T> request, CancellationToken cancellationToken)
    {
        await _cacheService.UpdateAsync(request.Key, request.Value, request.Expiration);
        return new CacheResult<T> { Action = true };
    }
}
