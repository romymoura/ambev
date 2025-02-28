using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cache;

public class UpdateCacheCommand : BaseCommand, IRequest<CacheResult>
{
    public string Key { get; set; }
    public TimeSpan Expiration { get; set; }
}
public class UpdateCacheCommand<T> : UpdateCacheCommand
{
    public T? Value { get; set; }
}