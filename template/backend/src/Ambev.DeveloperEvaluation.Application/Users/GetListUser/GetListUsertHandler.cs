using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetListUser;

/// <summary>
/// Handler for processing CreateUserCommand requests
/// </summary>
public class GetListUserHandler : IRequestHandler<GetListUserCommand, IEnumerable<GetListUserResult>>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public GetListUserHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetListUserResult>> Handle(GetListUserCommand request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetListAsync(request.Page, request.Size, cancellationToken);
        var result = _mapper.Map<IEnumerable<GetListUserResult>>(users);
        return result;
    }
}
