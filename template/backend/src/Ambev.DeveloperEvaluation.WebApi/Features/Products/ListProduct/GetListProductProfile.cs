using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Products.GetListProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// Profile for mapping between Application and API GetListProduct responses
/// </summary>
public class GetListProductProfile : Profile
{
    public GetListProductProfile()
    {
        CreateMap<PaginatedRequest, GetListProductCommand>()
            .BeforeMap((source, dist) =>
            {
                dist.Page = source.Page;
                dist.Size = source.Size;
            })
            .ReverseMap();

        CreateMap<GetListProductResult, GetProductListResponse>()
        .BeforeMap((source, dist) =>
        {
            dist.Price = source.Price;
            dist.Id = source.Id;
            dist.Title = source.Title;
            dist.Price = source.Price;
            dist.Description = source.Description;
            dist.Category = source.Category;
            dist.Image = source.Image;
            dist.Rating.Rate = source.Rating.Rate;
            dist.Rating.Count = source.Rating.Count;
        })
        .ReverseMap();
    }
}
