using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace SnapSell.Infrastructure.Services.I18nServices
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IMemoryCache _cache;

        public JsonStringLocalizerFactory(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(_cache);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(_cache);
        }
    }
}
