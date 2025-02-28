using MediatR;
namespace Ambev.DeveloperEvaluation.Application.Cache.Cart;

public class AddShoppingCommand : BaseCommand, IRequest<CacheResult> { }
public class AddShoppingCommand<T> : AddShoppingCommand
{
    public T? Value { get; set; }
}