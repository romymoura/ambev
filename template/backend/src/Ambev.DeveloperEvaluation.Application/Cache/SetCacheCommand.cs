using MediatR;
namespace Ambev.DeveloperEvaluation.Application.Cache;

public class SetCacheCommand : BaseCommand, IRequest<CacheResult>
{
}
public class SetCacheCommand<T> : SetCacheCommand
{
    public T? Value { get; set; }
}