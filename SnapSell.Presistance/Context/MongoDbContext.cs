using MongoDB.Driver;
using SnapSell.Domain.Attributes;
using SnapSell.Domain.Entities;
using SnapSell.Domain.Interfaces;
using System.Reflection;

namespace SnapSell.Presistance.Context
{
    namespace SnapSell.Infrastructure.Data
    {
        public sealed class MongoDbContext
        {
            private readonly IMongoDatabase _mongoDatabase;
            public MongoDbContext(IMongoClient client, string databaseName) => _mongoDatabase = client.GetDatabase(databaseName);

            //collections
            public IMongoCollection<Product> Products => GetCollection<Product>();
            public IMongoCollection<Category> Categories => GetCollection<Category>();
            public IMongoCollection<Brand> Brands => GetCollection<Brand>();
            public IMongoCollection<ProductAttributeDefinition> ProductAttributes => GetCollection<ProductAttributeDefinition>();

            //get any collection by type
            public IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : IDocument
            {
                var collectionName = GetCollectionName<TEntity>();
                return _mongoDatabase.GetCollection<TEntity>(collectionName);
            }

            //Gets the collection name from the BsonCollectionAttribute | class name
            private static string GetCollectionName<TEntity>() where TEntity : IDocument
            {
                var attribute = typeof(TEntity).GetCustomAttribute<BsonCollectionAttribute>();
                return attribute?.CollectionName ?? typeof(TEntity).Name.ToLower();
            }

            //creates indexes on application
            public void CreateIndexes()
            {
                var productKeys = Builders<Product>.IndexKeys
                    .Text(p => p.Description);

                Products.Indexes.CreateOne(new CreateIndexModel<Product>(productKeys));

            }
        }
    }
}