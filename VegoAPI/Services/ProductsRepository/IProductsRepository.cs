using System;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;

namespace VegoAPI.Services.ProductsRepository
{
    public interface IProductsRepository
    {
        Task<ProductShortResponse[]> GetAllProductsAsync();
        Task<ProductShortResponse[]> GetProductsByCategoriesAsync(int[] categoriesIds);
        Task<ProductShortResponse[]> GetProductsWithFilterAsync(FilteredProductsRequest filteredProductsRequest);
        Task<ProductDetailResponse> GetProductByIdAsync(int id);
        Task<byte[]> GetProductLowPhoto(Guid photoId);
        Task<byte[]> GetProductHighPhoto(Guid photoId);
        Task<string[]> GetProductPhotosPaths(int productId);
        Task AddProductAsync(AddProductRequest addProductRequest);
        Task LoadProductPhotoAsync(int productId, string lowResPhotoPath, string highResPhotoPath);
        Task LoadProductMainPhotoAsync(int productId, string lowResPhotoPath, string highResPhotoPath);
        Task SetProductMainPhotoAsync(int productId, Guid photoId);
        Task EditProductInfoAsync(EditEntityRequest editProductRequest);
        Task DeleteProductAsync(int id);
        Task RemoveProductPhotoAsync(Guid photoId);
    }
}
