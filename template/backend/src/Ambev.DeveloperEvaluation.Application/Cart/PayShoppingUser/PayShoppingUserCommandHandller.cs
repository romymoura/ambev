using Ambev.DeveloperEvaluation.Domain.Services;
using MediatR;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Cart.PayShoppingUser;

public class PayShoppingUserCommandHandller : IRequestHandler<PayShoppingUserCommand, PayShoppingUserResult>
{
    private readonly IRedisCacheService _cache;
    private readonly IMongoDbService _mdb;
    public PayShoppingUserCommandHandller(IRedisCacheService cache, IMongoDbService mdb)
    {
        _cache = cache;
        _mdb = mdb;
    }

    public async Task<PayShoppingUserResult?> Handle(PayShoppingUserCommand request, CancellationToken cancellationToken)
    {
        var item = await _cache.GetAsync<dynamic>(request.Key);
        if (item is dynamic)
        {
            string strJson = item is string ? item : JsonSerializer.Serialize(item);
            var result = await _mdb.AddListDocumentInCollectionAsync(
                request?.CollectionNameShoppingPayCart ?? string.Empty,
                strJson
            );
            if (result)
                await _cache.DeleteAsync(request?.Key ?? string.Empty);
        }
        return new PayShoppingUserResult { Action = !(item is null) };
    }
}
