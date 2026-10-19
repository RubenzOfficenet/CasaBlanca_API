using Core.Mappy.Extensions;

namespace CasaBlanca_API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services here
            services.AddMapper();

            return services;
        }
    }
}
