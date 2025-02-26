using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetListUser;

public class GetListUserCommand : IRequest<IEnumerable<GetListUserResult>>
{
    /// <summary>
    /// Current Page
    /// </summary>
    public int? Page { get; set; } = 0;

    /// <summary>
    /// Total per page
    /// </summary>
    public int? Size { get; set; } = 0;
}