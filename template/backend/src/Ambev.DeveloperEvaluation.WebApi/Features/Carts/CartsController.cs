using Ambev.DeveloperEvaluation.Application.Cache;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;


[ApiController]
[Route("api/[controller]")]
public class CartsController : BaseController
{
    public CartsController(IMediator mediator, IMapper mapper) : base(mediator, mapper) { }
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<SetCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [Authorize(Policy = "OnlyCustomer")]
    public async Task<IActionResult> SetShopping([FromBody] SetCartRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //request.Expiration = DateTime.Now.AddDays(10).TimeOfDay;
        request.Expiration = DateTime.Now.AddSeconds(10).TimeOfDay; // teste
        request.Key = $"shopping_{userId}";

        var command = new SetCacheCommand<List<dynamic>> { 
            Value = new List<dynamic> { request.Value }, 
            Expiration = request.Expiration.Value, 
            Key = request.Key 
        };
        var response = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<SetCartResponse>
        {
            Success = true,
            Message = "Products retrieved successfully",
            Data = new SetCartResponse { Action = response.Action }
        });
    }


}
