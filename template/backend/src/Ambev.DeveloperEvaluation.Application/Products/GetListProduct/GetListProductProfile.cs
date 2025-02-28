using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Application.Products.GetListProduct;

/// <summary>
/// Profile for mapping between ListProduct
/// </summary>
public class GetListProductProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser operation
    /// </summary>
    public GetListProductProfile()
    {
        CreateMap<Product, GetListProductResult>()
            .BeforeMap((source, dist) =>
            {
                dist.Id = source.Id;
                dist.Title = source.Title;
                dist.Price = source.Price;
                dist.Description = source.Description;
                dist.Category = source.Category;
                dist.Image = source.Image;
                dist.Rating = source.Rating;
                dist.CreateAt = source.CreatedAt;
                dist.UpdatedAt = source.UpdatedAt;
            })
            .ReverseMap();
    }
}
