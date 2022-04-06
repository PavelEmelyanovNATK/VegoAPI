using Microsoft.Extensions.DependencyInjection;
using VegoAPI.Services.ProductsRepository;

namespace VegoAPI.Services
{
    public static class VegoServicesExtensions
    {
        public static IServiceCollection AddProductsRepository(this IServiceCollection services)
            => services.AddScoped<IProductsRepository, ProductsFakeRepository>();
    }
}
