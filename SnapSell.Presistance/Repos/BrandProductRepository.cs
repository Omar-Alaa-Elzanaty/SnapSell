using MongoDB.Driver;
using SnapSell.Application.Extensions;
using SnapSell.Application.Interfaces.Repos;
using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Entities;
using SnapSell.Presistance.Context.SnapSell.Infrastructure.Data;
using System.Net;

namespace SnapSell.Presistance.Repos
{
    public sealed class BrandProductRepository : IBrandProductRepository
    {
        private readonly MongoDbContext _db;
        public BrandProductRepository(MongoDbContext db) => _db = db;
        public async Task AddProductToBrand(string productId, string brandId, bool isPrimary = false)
        {
            var productUpdate = Builders<Product>.Update
                .AddToSet(p => p.BrandIds, brandId);

            if (isPrimary)
            {
                productUpdate = productUpdate.Set(p => p.PrimaryBrandId, brandId);
            }

            await _db.Products.UpdateOneAsync(
                p => p.Id == productId,
                productUpdate);

            await _db.Brands.UpdateOneAsync(
                b => b.Id == brandId,
                Builders<Brand>.Update.AddToSet(b => b.ProductIds, productId));
        }

        public async Task<PaginatedResult<Product>> GetProductsWithBrands(
            List<string> productIds,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var filter = Builders<Product>.Filter.In(p => p.Id, productIds);
                var sort = sortBy.ToSortDefinition<Product>();

                var result = await _db.Products.ToMongoPaginatedListAsync(
                    filter,
                    sort,
                    pageNumber,
                    pageSize,
                    cancellationToken);

                if (result.Data is null || result.Data.Count is 0)
                {
                    return PaginatedResult<Product>.Create(
                        new List<Product>(),
                        0,
                        pageNumber,
                        pageSize);
                }

                await LoadRelatedBrands(result.Data, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return await PaginatedResult<Product>.FailureAsync(
                    $"Error getting products with brands: {ex.Message}",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PaginatedResult<Brand>> GetBrandsWithProducts(
            List<string> brandIds,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var filter = Builders<Brand>.Filter.In(b => b.Id, brandIds);
                var sort = sortBy.ToSortDefinition<Brand>();

                var result = await _db.Brands.ToMongoPaginatedListAsync(
                    filter,
                    sort,
                    pageNumber,
                    pageSize,
                    cancellationToken);

                if (result.Data == null || result.Data.Count is 0)
                {
                    return PaginatedResult<Brand>.Create(
                        new List<Brand>(),
                        0,
                        pageNumber,
                        pageSize);
                }
                await LoadRelatedProducts(result.Data, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                return await PaginatedResult<Brand>.FailureAsync(
                    $"Error getting brands with products: {ex.Message}",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PaginatedResult<Product>> GetProductsByBrand(
            string brandId,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var brand = await _db.Brands
                    .Find(b => b.Id == brandId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (brand?.ProductIds is null || brand.ProductIds.Count is 0)
                {
                    return PaginatedResult<Product>.Create(new List<Product>(), 0, pageNumber, pageSize);
                }

                var filter = Builders<Product>.Filter.In(p => p.Id, brand.ProductIds);
                var sort = sortBy.ToSortDefinition<Product>();

                return await _db.Products.ToMongoPaginatedListAsync(
                    filter,
                    sort,
                    pageNumber,
                    pageSize,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                return await PaginatedResult<Product>.FailureAsync(
                    $"Error getting products by brand: {ex.Message}",
                    HttpStatusCode.InternalServerError);
            }
        }

        public async Task<PaginatedResult<Brand>> GetBrandsByProduct(
            string productId,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var product = await _db.Products
                    .Find(p => p.Id == productId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (product?.BrandIds is null || product.BrandIds.Count is 0)
                {
                    return PaginatedResult<Brand>.Create(
                        new List<Brand>(),
                        0,
                        pageNumber,
                        pageSize);
                }

                var filter = Builders<Brand>.Filter.In(b => b.Id, product.BrandIds);
                var sort = sortBy.ToSortDefinition<Brand>();

                return await _db.Brands.ToMongoPaginatedListAsync(
                    filter,
                    sort,
                    pageNumber,
                    pageSize,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                return await PaginatedResult<Brand>.FailureAsync(
                    $"Error getting brands by product: {ex.Message}",
                    HttpStatusCode.InternalServerError);
            }
        }

        private async Task LoadRelatedBrands(List<Product> products, CancellationToken cancellationToken)
        {
            var brandIds = products
                .SelectMany(p => p.BrandIds)
                .Distinct()
                .ToList();

            var brands = await _db.Brands
                .Find(b => brandIds.Contains(b.Id))
                .ToListAsync(cancellationToken);

            foreach (var product in products)
            {
                product.Brands = brands
                    .Where(b => product.BrandIds.Contains(b.Id))
                    .ToList();
            }
        }

        private async Task LoadRelatedProducts(List<Brand> brands, CancellationToken cancellationToken)
        {
            var productIds = brands
                .SelectMany(b => b.ProductIds)
                .Distinct()
                .ToList();

            var products = await _db.Products
                .Find(p => productIds.Contains(p.Id))
                .ToListAsync(cancellationToken);

            foreach (var brand in brands)
            {
                brand.Products = products
                    .Where(p => brand.ProductIds.Contains(p.Id))
                    .ToList();
            }
        }
    }
}