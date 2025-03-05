using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class GetCacheCommand : BaseCommand, IRequest<CacheResult>
{
    public string Key { get; set; }
}

public class GetCacheCommand<T> : GetCacheCommand, IRequest<CacheResult<T>>
{
}