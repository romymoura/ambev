using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Cache;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<SetCartRequest, SetCacheCommand>();
    }
}
