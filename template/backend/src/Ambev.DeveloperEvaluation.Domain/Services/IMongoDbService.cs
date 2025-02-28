namespace Ambev.DeveloperEvaluation.Domain.Services;

public interface IMongoDbService
{
    Task<bool> AddListDocumentInCollectionAsync(string collectionName, string jsonArray);
}
