using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json.Nodes;

namespace Ambev.DeveloperEvaluation.NoSql.MDb;

public class MongoDbService : IMongoDbService
{
    private readonly IMongoDatabase _database;

    public MongoDbService(IOptions<MongoSettings> mongoSettings)
    {
        var client = new MongoClient(mongoSettings.Value.ConnectionString);
        _database = client.GetDatabase(mongoSettings.Value.DatabaseName);
    }

    private async Task CreateCollection(string collectionName)
    {
        var collections = await _database.ListCollectionNamesAsync();
        var collectionList = await collections.ToListAsync();
        if (!collectionList.Contains(collectionName))
            await _database.CreateCollectionAsync(collectionName);
    }

    public async Task<bool> AddListDocumentInCollectionAsync(string collectionName, string jsonArray)
    {
        await CreateCollection(collectionName);
        var bsonArray = BsonSerializer.Deserialize<BsonArray>(jsonArray);
        var collection = _database.GetCollection<BsonDocument>(collectionName);
        var bsonDocuments = new List<BsonDocument>();
        foreach (var item in bsonArray)
            bsonDocuments.Add(item.AsBsonDocument);
        if (bsonDocuments.Count > 0)
            await collection.InsertManyAsync(bsonDocuments);
        return true;
    }
}