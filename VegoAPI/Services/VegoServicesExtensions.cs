using Microsoft.Extensions.DependencyInjection;
using VegoAPI.Services.OrdersRepository;
using VegoAPI.Services.PhotosHandler;
using VegoAPI.Services.ProductsRepository;
using VegoAPI.Services.ProductTypesRepository;

namespace VegoAPI.Services
{
    public static class VegoServicesExtensions
    {
        public static IServiceCollection AddProductsRepository(this IServiceCollection services)
            => services.AddScoped<IProductsRepository, ProductsRealRepository>();

        public static IServiceCollection AddProductTypesRepository(this IServiceCollection services)
            => services.AddScoped<IProductTypesRepository, ProductTypesRealRepository>();

        public static IServiceCollection AddPhotosHandler(this IServiceCollection services)
            => services.AddScoped<IPhotosHandler, PhotosHandler.PhotosHandler>();

        public static IServiceCollection AddOrdersRepository(this IServiceCollection services)
            => services.AddScoped<IOrdersRepository, OrdersRepository.OrdersRepository>();
    }
}
