using MediatR;
namespace Ambev.DeveloperEvaluation.Application.Cache;
public class DeleteCacheCommand : BaseCommand, IRequest<CacheResult>{}
public class DeleteCacheCommand<T> : DeleteCacheCommand{}