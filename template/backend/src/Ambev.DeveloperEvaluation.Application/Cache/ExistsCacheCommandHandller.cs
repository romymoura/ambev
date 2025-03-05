using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class ExistsCacheCommandHandller : IRequestHandler<ExistsCacheCommand, CacheResult>
{
    private readonly IRedisCacheService _cacheService;
    public ExistsCacheCommandHandller(IRedisCacheService cacheService)
    {
        _cacheService = cacheService;
    }
    public async Task<CacheResult> Handle(ExistsCacheCommand request, CancellationToken cancellationToken)
    {
        var cacheItem = await _cacheService.GetAsync<dynamic>(request.Key);
        return new CacheResult { Action = cacheItem != null };
    }
}
