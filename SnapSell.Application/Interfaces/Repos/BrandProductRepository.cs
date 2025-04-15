using SnapSell.Domain.Dtos.ResultDtos;
using SnapSell.Domain.Entities;


namespace SnapSell.Application.Interfaces.Repos
{
    public interface IBrandProductRepository
    {

        Task AddProductToBrand(string productId, string brandId, bool isPrimary = false);

        Task<PaginatedResult<Product>> GetProductsWithBrands(
            List<string> productIds,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            CancellationToken cancellationToken = default);

        Task<PaginatedResult<Brand>> GetBrandsWithProducts(
            List<string> brandIds,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            CancellationToken cancellationToken = default);

        Task<PaginatedResult<Product>> GetProductsByBrand(
            string brandId,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            CancellationToken cancellationToken = default);
        Task<PaginatedResult<Brand>> GetBrandsByProduct(
            string productId,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            CancellationToken cancellationToken = default);

    }
}