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
        Task<ProductDetailResponse> GetProductByIdAsync(Guid id);
        Task<Guid> AddProductAsync(AddProductRequest addProductRequest);
        Task<Guid> AddProductPhotoAsync(AddProductPhotoRequest addProductImageRequest);
        Task SetProductMainPhotoAsync(ProductToPhotoRequest setProductMainPhotoRequest);
        Task EditProductInfoAsync(EditEntityWithGuidRequest editProductRequest);
        Task DeleteProductAsync(Guid id);
        Task DeleteProductPhotoAsync(Guid photoId);
        Task<PhotoResponse[]> GetAllProductPhotosAsync(Guid productId);
        Task<PhotoResponse[]> GetAllProductPhotosAsync(int page);
        Task<PhotoResponse[]> GetAllProductPhotosAsync();
    }
}
