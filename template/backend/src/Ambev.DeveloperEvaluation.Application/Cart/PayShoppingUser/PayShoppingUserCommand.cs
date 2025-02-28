using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Cart.PayShoppingUser;

public class PayShoppingUserCommand : BaseCommand, IRequest<PayShoppingUserResult>
{
    public string? CollectionNameShoppingPayCart { get; set; }
}
