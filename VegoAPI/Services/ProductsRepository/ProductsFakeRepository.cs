using System.Linq;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;
using VegoAPI.Utils;

namespace VegoAPI.Services.ProductsRepository
{
    public class ProductsFakeRepository
    {
        private readonly ProductShortResponse[] _products = new[]
        {
            new ProductShortResponse
            {
                Id = 1,
                Title = "Продукт 1",
                IsActive = true
            },
            new ProductShortResponse
            {
                Id = 2,
                Title = "Продукт 2",
                IsActive = true
            },
            new ProductShortResponse
            {
                Id = 3,
                Title = "Продукт 3",
                IsActive = true
            },
            new ProductShortResponse
            {
                Id = 4,
                Title = "Продукт 4",
                IsActive = true
            },
            new ProductShortResponse
            {
                Id = 5,
                Title = "Продукт 5",
                IsActive = true
            }
        };

        public Task AddProductAsync(AddProductRequest addProductRequest)
        {
            return Task.CompletedTask;
        }

        public Task DeleteProductAsync(int productId)
        {
            return Task.CompletedTask;
        }

        public Task EditProductInfoAsync(EditEntityRequest editProductRequest)
        {
            return Task.CompletedTask;
        }

        public async Task<ProductShortResponse[]> GetAllProductsAsync()
            => await Task.FromResult(_products);
        

        public async Task<ProductDetailResponse> GetProductByIdAsync(int id)
            => await Task.FromResult(_products
                .FirstOrDefault(p => p.Id == id)
                ?.Let(p => 
                new ProductDetailResponse
                {
                    Id = p.Id

                }));

        public async Task<ProductShortResponse[]> GetProductsByCategoriesAsync(int[] categoriesIds)
        {
            return await Task.FromResult(_products);
        }
    }
}
