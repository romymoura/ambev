using MediatR;
namespace Ambev.DeveloperEvaluation.Application.Cache;
public class ExistsCacheCommand : BaseCommand, IRequest<CacheResult> { }

