using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductCommand : BaseCommand, IRequest<CreateProductResult>
{
    public int Page { get; set; }
    public int Size { get; set; }
}