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
        if (!string.IsNullOrWhiteSpace(collectionName))
            return _database.GetCollection<T>(collectionName);

        var attribute = typeof(T).GetCustomAttribute<CollectionNameAttribute>();
        return _database.GetCollection<T>(attribute is not null ? attribute.Name : Pluralize(typeof(T).Name));
    }

    private string Pluralize(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        return name switch
        {
            _ when name.EndsWith("y") => name[..^1] + "ies",
            _ when name.EndsWith("s") => name + "es",
            _ => name + "s"
        };
    }
}
