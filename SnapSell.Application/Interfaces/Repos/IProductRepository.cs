using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Entities;

namespace SnapSell.Application.Interfaces.Repos
{
    public interface IProductRepository
    {
        // Single entity operations
        Task<Product> GetByIdAsync(string id);
        Task<Product> CreateAsync(Product product);
        Task UpdateAsync(string id, Product product);
        Task DeleteAsync(string id);

        //paging queries with sorting
        Task<PaginatedResult<Product>> GetByBrandAsync(
            string brandId,
            int pageNumber = 1,
            int pageSize = 20,
            string sortBy = "");

        Task<PaginatedResult<Product>> SearchAsync(
            string query,
            int pageNumber = 1,
            int pageSize = 20,
            string sortBy = "");

        Task<PaginatedResult<Product>> GetFeaturedProductsAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "");

        Task<PaginatedResult<Product>> GetRelatedProductsAsync(
            string productId,
            int pageNumber = 1,
            int pageSize = 5,
            string sortBy = "");

        //other
        Task AddBrandAsync(string productId, string brandId, bool isPrimary = false);
        Task RemoveBrandAsync(string productId, string brandId);
        Task<List<Product>> GetByBrandAsync(string brandId, int limit = 50);
        Task<List<Product>> SearchAsync(string query, int limit = 20);
    }
}