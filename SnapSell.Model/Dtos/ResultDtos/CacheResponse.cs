using SnapSell.Domain.Models.Interfaces;

namespace SnapSell.Domain.Dtos.ResultDtos
{
    public class CacheResponse
    {
        public GeneralCahce GeneralCache { get; set; }
        public UserCache UserCache { get; set; }
    }
    public class GeneralCahce
    {
        public CategoryCache CategoryCache { get; set; }
        public BrandCache BrandCache { get; set; }
    }
    public class UserCache
    {
        public ProfileCache ProfileCache { get; set; }
    }
    public class CategoryCache : IGeneralCache
    {
        public int Version { get; set; }
        public string LastUpdated { get; set; }
    }
    public class BrandCache : IGeneralCache
    {
        public int Version { get; set; }
        public string LastUpdated { get; set; }
    }
    public class ProfileCache : IUserCache
    {
        public int Version { get; set; }
        public string LastUpdated { get; set; }
    }
}
