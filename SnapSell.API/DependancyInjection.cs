using System.Runtime.CompilerServices;

namespace SnapSell.API
{
    public static class DependancyInjection
    {
        public static IServiceCollection DepedencyInjectionService(this IServiceCollection services)
        {
            return services;
        }
    }
}
