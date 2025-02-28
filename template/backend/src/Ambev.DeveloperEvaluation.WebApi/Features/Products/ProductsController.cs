using Ambev.DeveloperEvaluation.Application.Products.GetListProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AllManagerOrAdminOrCustomer")]
public class ProductsController : BaseController
{
    public ProductsController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetProductListResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProducts([FromQuery] PaginatedRequest paginated, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<GetListProductCommand>(paginated);
        var response = await _mediator.Send(command, cancellationToken);
        var data = _mapper.Map<IEnumerable<GetProductListResponse>>(response);

        return Ok(new ApiResponseWithData<IEnumerable<GetProductListResponse>>
        {
            Success = true,
            Message = "Products retrieved successfully",
            Data = data
        });
    }
}
