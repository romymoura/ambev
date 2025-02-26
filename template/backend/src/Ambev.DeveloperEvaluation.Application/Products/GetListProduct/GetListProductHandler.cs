using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Application.Products.GetListProduct;

/// <summary>
/// Handler for processing CreateUserCommand requests
/// </summary>
public class GetListProductHandler : IRequestHandler<GetListProductCommand, IEnumerable<GetListProductResult>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetListProductHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetListProductResult>> Handle(GetListProductCommand request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetListAsync(request.Page, request.Size, cancellationToken);
        var result = _mapper.Map<IEnumerable<GetListProductResult>>(products);
        return result;
    }

    ///// <summary>
    ///// Handles the CreateUserCommand request
    ///// </summary>
    ///// <param name="command">The CreateUser command</param>
    ///// <param name="cancellationToken">Cancellation token</param>
    ///// <returns>The created user details</returns>
    //public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    //{
    //    var validator = new CreateProductCommandValidator();
    //    var validationResult = await validator.ValidateAsync(command, cancellationToken);

    //    if (!validationResult.IsValid)
    //        throw new ValidationException(validationResult.Errors);

    //    //var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
    //    //if (existingUser != null)
    //    //    throw new InvalidOperationException($"User with email {command.Email} already exists");

    //    var user = _mapper.Map<User>(command);
    //    user.Password = _passwordHasher.HashPassword(command.Password);

    //    var createdUser = await _userRepository.CreateAsync(user, cancellationToken);
    //    var result = _mapper.Map<CreateProductResult>(createdUser);
    //    return result;
    //}
}
