using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;

namespace VegoAPI.Services.ProductsRepository
{
    public interface IProductsRepository
    {
        Task<ProductShortResponse[]> GetAllProductsAsync();
        Task<ProductShortResponse> GetProductByIdAsync(int id);
        Task AddProductAsync(AddProductRequest addProductRequest);
        Task EditProductInfoAsync(EditEntityRequest editProductRequest);
        Task DeleteProductAsync(int id);
    }
}
