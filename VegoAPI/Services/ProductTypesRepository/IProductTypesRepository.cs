using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;

namespace VegoAPI.Services.ProductTypesRepository
{
    public interface IProductTypesRepository
    {
        Task<ProductTypeResponse[]> GetAllProductTypesAsync();
        Task<ProductTypeResponse> GetProductTypeByIdAsync(int id);
        Task AddProductTypeAsync(AddProductTypeRequest addProductTypeRequest);
        Task EditProductTypeAsync(EditEntityWithIntIdRequest editProductTypeRequest);
        Task DeleteProductTypeAsync(int productTypeId);
    }
}
