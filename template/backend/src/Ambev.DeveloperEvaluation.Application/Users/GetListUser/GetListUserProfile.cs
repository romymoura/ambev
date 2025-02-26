using Ambev.DeveloperEvaluation.Application.Products.GetListUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.GetListProduct;

/// <summary>
/// Profile for mapping between User entity and CreateUserResponse
/// </summary>
public class GetListUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser operation
    /// </summary>
    public GetListUserProfile()
    {
        CreateMap<User, GetListUserResult>()
            .BeforeMap((source, dist) =>
            {
                dist.Id = source.Id;
                dist.Username = source.Username;
                dist.Email = source.Email;
                dist.Phone = source.Phone;
                dist.Password = source.Password;
                dist.Role = source.Role;
                dist.Status = source.Status;
                dist.CreatedAt = source.CreatedAt;
                dist.UpdatedAt = source.UpdatedAt;
            })
            .ReverseMap();
    }
}
