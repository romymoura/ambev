using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class DeleteCacheCommandHandler : IRequestHandler<DeleteCacheCommand, CacheResult>
{
    private readonly IRedisCacheService _cacheService;
    public DeleteCacheCommandHandler(IRedisCacheService cacheService)
    {
        _cacheService = cacheService;
    }
    public async Task<CacheResult> Handle(DeleteCacheCommand request, CancellationToken cancellationToken)
    {
        await _cacheService.DeleteAsync(request.Key);
        return new CacheResult { Action = true };
    }
}