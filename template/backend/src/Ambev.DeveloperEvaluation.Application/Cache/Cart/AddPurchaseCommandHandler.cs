//using Ambev.DeveloperEvaluation.Domain.Services;
//using MediatR;
//using System.Text.Json;

//namespace Ambev.DeveloperEvaluation.Application.Cache.Cart;

//public class AddShoppingCommandHandler<T> : IRequestHandler<AddShoppingCommand<T>, CacheResult>
//{
//    private readonly IRedisCacheService _cacheService;
//    public AddShoppingCommandHandler(IRedisCacheService cacheService)
//    {
//        _cacheService = cacheService;
//    }

//    public async Task<CacheResult> Handle(AddShoppingCommand<T> request, CancellationToken cancellationToken)
//    {
//        if (request.Value is null)
//            return new CacheResult { Action = false };

//        var purchase = await _cacheService.GetAsync<dynamic>(request.Key);
//        if (purchase is null)
//            await _cacheService.SetAsync<dynamic>(request.Key, request.Value, request.Expiration);


//        if (purchase is List<dynamic> purchaseList)
//        {
//            string requestValueJson = JsonSerializer.Serialize(request.Value);
//            using JsonDocument requestDoc = JsonDocument.Parse(requestValueJson);

//            // Se o elemento raiz é um objeto, não um array
//            string requestId = string.Empty;
//            using JsonDocument requestDoc rating = null;
//            if (requestDoc.RootElement.ValueKind == JsonValueKind.Object)
//            {
//                if (requestDoc.RootElement.TryGetProperty("id", out JsonElement idElement))
//                    requestId = idElement.ToString();

//                if (requestDoc.RootElement.TryGetProperty("rating", out JsonElement ratingElement))
//                    rating = ratingElement.ToString();
//            }

//            if (!string.IsNullOrEmpty(requestId))
//            {
//                int existingIndex = -1;

//                // Procure pelo item com o mesmo ID
//                for (int i = 0; i < purchaseList.Count; i++)
//                {
//                    var currentItem = purchaseList[i];

//                    // Se o item atual é um JsonElement
//                    if (currentItem is JsonElement element)
//                    {
//                        if (element.ValueKind == JsonValueKind.Object &&
//                            element.TryGetProperty("id", out JsonElement idElement) &&
//                            idElement.ToString() == requestId)
//                        {
//                            existingIndex = i;
//                            break;
//                        }
//                    }
//                    // Se o item já está como um objeto, serialize para JSON e verifique
//                    else
//                    {
//                        string itemJson = JsonSerializer.Serialize(currentItem);
//                        using JsonDocument itemDoc = JsonDocument.Parse(itemJson);

//                        if (itemDoc.RootElement.ValueKind == JsonValueKind.Object &&
//                            itemDoc.RootElement.TryGetProperty("id", out JsonElement idElement) &&
//                            idElement.ToString() == requestId)
//                        {
//                            existingIndex = i;
//                            break;
//                        }
//                    }
//                }
//            }
     

//            if (requestDoc.RootElement.ValueKind == JsonValueKind.Array)
//            {
//                //else
//                //{

//                //    }

//                //    if (existingIndex >= 0)
//                //    {
//                //        // Substitui o item existente
//                //        purchaseList[existingIndex] = request.Value;
//                //    }
//                //    else
//                //    {
//                //        // Adiciona novo item
//                //        purchaseList.Add(request.Value);
//                //    }
//                //}

//                // Atualiza o cache

//            }
//            await _cacheService.SetAsync<dynamic>(request.Key, purchase, request.Expiration);
//            return new CacheResult { Action = true };
//        }
//    }
//}
