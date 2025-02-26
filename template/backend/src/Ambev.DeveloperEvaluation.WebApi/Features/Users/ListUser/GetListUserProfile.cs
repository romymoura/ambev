using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Products.GetListProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetListUser;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Profile for mapping between Application and API GetListProduct responses
/// </summary>
public class GetListUserProfile : Profile
{
    public GetListUserProfile()
    {
        CreateMap<PaginatedRequest, GetListUserCommand>()
            .BeforeMap((source, dist) =>
            {
                dist.Page = source.Page;
                dist.Size = source.Size;
            })
            .ReverseMap();

        CreateMap<GetListUserResult, GetUserListResponse>()
        .BeforeMap((source, dist) =>
        {
            dist.Id = source.Id;
            dist.Username = source.Username;
            dist.Email = source.Email;
            dist.Phone = source.Phone;
            dist.Role = source.Role.ToString();
        })
        .ReverseMap();
    }
}
