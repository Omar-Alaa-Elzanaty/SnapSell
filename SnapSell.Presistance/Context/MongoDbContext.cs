using System.Reflection;
using MongoDB.Driver;
using SnapSell.Domain.Attributes;

namespace SnapSell.Presistance.Context;


public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient client, IMongoDbSettings settings)
    {
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName = null)
    {
        var attribute = typeof(T).GetCustomAttribute<CollectionNameAttribute>();
        return _database.GetCollection<T>(attribute?.Name);
    }
}
