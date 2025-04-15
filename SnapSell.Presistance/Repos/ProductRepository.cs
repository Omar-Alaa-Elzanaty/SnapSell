using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SnapSell.Application.Extensions;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Entities;
using SnapSell.Presistance.Context.SnapSell.Infrastructure.Data;

namespace SnapSell.Presistance.Repos
{
    public sealed class ProductRepository : IProductRepository
    {
        private readonly MongoDbContext _context;
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Brand> _brands;

        public ProductRepository(MongoDbContext context)
        {
            _context = context;
            _products = context.Products;
            _brands = context.Brands;
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
            if (product?.BrandIds?.Count > 0)
            {
                product.Brands = await _brands.Find(b => product.BrandIds.Contains(b.Id)).ToListAsync();
            }
            return product;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task UpdateAsync(string id, Product product)
        {
            await _products.ReplaceOneAsync(p => p.Id == id, product);
        }

        public async Task DeleteAsync(string id)
        {
            await _brands.UpdateManyAsync(
                Builders<Brand>.Filter.Where(b => b.ProductIds.Contains(id)),
                Builders<Brand>.Update.Pull(b => b.ProductIds, id));

            await _products.DeleteOneAsync(p => p.Id == id);
        }

        public async Task<PaginatedResult<Product>> GetByBrandAsync(
            string brandId,
            int pageNumber = 1,
            int pageSize = 20,
            string sortBy = "")
        {
            var filter = Builders<Product>.Filter.Where(p => p.BrandIds.Contains(brandId));
            var sort = string.IsNullOrEmpty(sortBy)
                ? Builders<Product>.Sort.Ascending("_id")
                : sortBy.ToSortDefinition<Product>();

            return await _products.ToMongoPaginatedListAsync(
                filter,
                sort,
                pageNumber,
                pageSize);
        }

        public async Task<List<Product>> GetByBrandAsync(string brandId, int limit = 50)
        {
            return await _products.Find(p => p.BrandIds.Contains(brandId))
                                 .Limit(limit)
                                 .ToListAsync();
        }

        public async Task<PaginatedResult<Product>> SearchAsync(
            string query,
            int pageNumber = 1,
            int pageSize = 20,
            string sortBy = "")
        {
            // Create the base query
            var filter = Builders<Product>.Filter.Or(
                Builders<Product>.Filter.Where(p => p.Name.Contains(query)),
                Builders<Product>.Filter.Where(p => p.Description.Contains(query))
            );

            // Convert sort string to SortDefinition
            var sort = sortBy.ToSortDefinition<Product>();

            // Use the IMongoCollection extension method for pagination
            return await _products.ToMongoPaginatedListAsync(
                filter,
                sort,
                pageNumber,
                pageSize);
        }

        public async Task<List<Product>> SearchAsync(string query, int limit = 20)
        {
            return await _products.Find(p =>
                    p.Name.Contains(query) ||
                    p.Description.Contains(query))
                .Limit(limit)
                .ToListAsync();
        }

        public async Task<PaginatedResult<Product>> GetFeaturedProductsAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "")
        {
            var filter = Builders<Product>.Filter.Where(p => p.IsFeatured);
            var sort = string.IsNullOrEmpty(sortBy)
                ? Builders<Product>.Sort.Descending(p => p.CreatedAt)
                : sortBy.ToSortDefinition<Product>();

            return await _products.ToMongoPaginatedListAsync(
                filter,
                sort,
                pageNumber,
                pageSize);
        }

        public async Task<PaginatedResult<Product>> GetRelatedProductsAsync(
            string productId,
            int pageNumber = 1,
            int pageSize = 5,
            string sortBy = "")
        {
            var product = await GetByIdAsync(productId);
            if (product?.BrandIds == null || !product.BrandIds.Any())
            {
                return PaginatedResult<Product>.Create(new List<Product>(), 0, pageNumber, pageSize);
            }

            var filter = Builders<Product>.Filter.And(
                Builders<Product>.Filter.AnyIn(p => p.BrandIds, product.BrandIds),
                Builders<Product>.Filter.Ne(p => p.Id, productId));

            var sort = string.IsNullOrEmpty(sortBy)
                ? Builders<Product>.Sort.Ascending("_id")
                : sortBy.ToSortDefinition<Product>();

            return await _products.ToMongoPaginatedListAsync(
                filter,
                sort,
                pageNumber,
                pageSize);
        }

        public async Task AddBrandAsync(string productId, string brandId, bool isPrimary = false)
        {
            var update = Builders<Product>.Update.AddToSet(p => p.BrandIds, brandId);
            if (isPrimary)
            {
                update = update.Set(p => p.PrimaryBrandId, brandId);
            }

            await _products.UpdateOneAsync(
                p => p.Id == productId,
                update);

            await _brands.UpdateOneAsync(
                b => b.Id == brandId,
                Builders<Brand>.Update.AddToSet(b => b.ProductIds, productId));
        }

        public async Task RemoveBrandAsync(string productId, string brandId)
        {
            // Create update definition for primary brand
            var update = Builders<Product>.Update
                .Pull(p => p.BrandIds, brandId);

            // Check if we need to clear primary brand
            var product = await GetByIdAsync(productId);
            if (product?.PrimaryBrandId == brandId)
            {
                update = update.Set(p => p.PrimaryBrandId, null);
            }

            await _products.UpdateOneAsync(
                p => p.Id == productId,
                update);

            await _brands.UpdateOneAsync(
                b => b.Id == brandId,
                Builders<Brand>.Update.Pull(b => b.ProductIds, productId));
        }
    }
}