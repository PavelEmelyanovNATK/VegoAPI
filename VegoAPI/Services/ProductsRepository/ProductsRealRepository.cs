using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;
using VegoAPI.Utils;
using VegoAPI.VegoAPI.Models.DBEntities;
using VegoAPI.VegoAPI.Services.DBContext;

namespace VegoAPI.Services.ProductsRepository
{
    public class ProductsRealRepository : IProductsRepository
    {
        private readonly VegoCityServerDBContext _dao;

        private const int PHOTOS_PER_PAGE = 20;

        public ProductsRealRepository(VegoCityServerDBContext dao)
        {
            _dao = dao;
        }

        public async Task<Guid> AddProductAsync(AddProductRequest addProductRequest)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Title = addProductRequest.Title,
                CategoryId = addProductRequest.ProductTypeId,
                Price = addProductRequest.Price,
                Description = addProductRequest.Description
            };

            await _dao.Products.AddAsync(product);
            await _dao.SaveChangesAsync();

            return product.Id;
        }

        public async Task<Guid> AddProductPhotoAsync(AddProductPhotoRequest addProductImageRequest)
        {
            var product = await _dao.Products.FindAsync(addProductImageRequest.ProductId);

            if (product is null)
                throw new Exception("Продукта не существует");

            var photo = new ProductPhoto
            {
                Id = Guid.NewGuid(),
                LowResPhotoPath = addProductImageRequest.LowImagePath,
                HighResPhotoPath = addProductImageRequest.HighImagePath
            };

            product.ProductPhotos.Add(photo);

            await _dao.SaveChangesAsync();

            return photo.Id;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _dao.Products.FindAsync(id);

            if (product is null) return;

            var photos = product.ProductPhotos.ToArray();

            _dao.ProductPhotos.RemoveRange(photos);
            _dao.Products.Remove(product);
            await _dao.SaveChangesAsync();
        }

        public async Task EditProductInfoAsync(EditEntityWithGuidRequest editProductRequest)
        {
            var product = await _dao.Products.FindAsync(editProductRequest.EntityId);

            if (product is null)
                return;

            editProductRequest.ChangedFields.GetValueOrDefault("Title")
            ?.Let(title => product.Title = title);

            editProductRequest.ChangedFields.GetValueOrDefault("CategoryId")
            ?.Let(productTypeId => product.CategoryId = Convert.ToInt32(productTypeId));

            editProductRequest.ChangedFields.GetValueOrDefault("Price")
            ?.Let(price => product.Price = Convert.ToDouble(price));

            editProductRequest.ChangedFields.GetValueOrDefault("Description")
            ?.Let(description => product.Description = description);

            editProductRequest.ChangedFields.GetValueOrDefault("IsActive")
            ?.Let(accesebility => product.IsActive = Convert.ToBoolean(accesebility));

            await _dao.SaveChangesAsync();
        }

        public async Task<ProductShortResponse[]> GetAllProductsAsync()
        {
            var products = await _dao.Products.ToArrayAsync();
            var productsCount = products.Length;

            var productsResponse = new ProductShortResponse[productsCount];

            for (int i = 0; i < productsCount; i++)
                productsResponse[i] = new ProductShortResponse
                {
                    Id = products[i].Id,
                    Title = products[i].Title,
                    Category = products[i].Category.Name,
                    CategoryId = products[i].CategoryId,
                    Price = products[i].Price,
                    IsActive = true,
                    ImagePath = products[i].MainPhotoId == null
                    ? ""
                    : (await _dao.ProductPhotos.FindAsync(products[i].MainPhotoId)).LowResPhotoPath
                };

            return productsResponse;
        }

        public async Task<ProductDetailResponse> GetProductByIdAsync(Guid id)
        { 
            var product = await _dao.Products.FindAsync(id);

            if(product is null)
                return null;

            return new ProductDetailResponse
            {
                Id = product.Id,
                Title = product.Title,
                Category = product.Category.Name,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Price = product.Price,
                IsActive = product.IsActive,
                Photos = product.ProductPhotos.Select(p => 
                new PhotoResponse 
                { 
                    PhotoId = p.Id, 
                    LowResPath = p.LowResPhotoPath,
                    HighResPath = p.HighResPhotoPath
                })
                .ToArray(),
                MainPhotoId = product.MainPhotoId
            };
        }
        public async Task<ProductShortResponse[]> GetProductsByCategoriesAsync(int[] categoriesIds)
        {
            Product[] products;

            if (categoriesIds.Length > 0)
                products = await _dao.Products.Where(p => categoriesIds.Contains(p.CategoryId)).ToArrayAsync();
            else
                products = await _dao.Products.ToArrayAsync();

            var productsCount = products.Length;

            var productsResponse = new ProductShortResponse[productsCount];

            for (int i = 0; i < productsCount; i++)
                productsResponse[i] = new ProductShortResponse
                {
                    Id = products[i].Id,
                    Title = products[i].Title,
                    Category = products[i].Category.Name,
                    CategoryId = products[i].CategoryId,
                    Price = products[i].Price,
                    IsActive = products[i].IsActive,
                    ImagePath = products[i].MainPhotoId == null
                    ? ""
                    : (await _dao.ProductPhotos.FindAsync(products[i].MainPhotoId))?.LowResPhotoPath ?? ""
                };

            return productsResponse;
        }

        public async Task<ProductShortResponse[]> GetProductsWithFilterAsync(FilteredProductsRequest filteredProductsRequest)
        {
            Product[] products;

            if (filteredProductsRequest.CategoriesIds is not null && filteredProductsRequest.CategoriesIds.Length > 0)
                products = await _dao.Products.Where(p => 
                filteredProductsRequest.CategoriesIds.Contains(p.CategoryId) && p.Title.Contains(filteredProductsRequest.Filter))
                    .ToArrayAsync();
            else
                products = await _dao.Products.Where(p => p.Title.Contains(filteredProductsRequest.Filter ?? "")).ToArrayAsync();

            var productsCount = products.Length;

            var productsResponse = new ProductShortResponse[productsCount];

            for (int i = 0; i < productsCount; i++)
                productsResponse[i] = new ProductShortResponse
                {
                    Id = products[i].Id,
                    Title = products[i].Title,
                    Category = products[i].Category.Name,
                    CategoryId = products[i].CategoryId,
                    Price = products[i].Price,
                    IsActive = products[i].IsActive,
                    ImagePath = products[i].MainPhotoId == null
                    ? ""
                    : (await _dao.ProductPhotos.FindAsync(products[i].MainPhotoId))?.LowResPhotoPath ?? ""
                };

            return productsResponse;
        }

        public async Task SetProductMainPhotoAsync(ProductToPhotoRequest setProductMainPhotoRequest)
        {
            var product = await _dao.Products.FindAsync(setProductMainPhotoRequest.ProductId);

            if (product is null)
                return;

            var photo = product.ProductPhotos.FirstOrDefault(p => p.Id == setProductMainPhotoRequest.PhotoId);

            if (photo is null)
                return;

            product.MainPhotoId = photo.Id;

            await _dao.SaveChangesAsync();
        }

        public async Task DeleteProductPhotoAsync(Guid photoId)
        {
            var photo = await _dao.ProductPhotos.FindAsync(photoId);
            if (photo is null)
                return;

            _dao.ProductPhotos.Remove(photo);
            await _dao.SaveChangesAsync();
        }

        public async Task<PhotoResponse[]> GetAllProductPhotosAsync(Guid productId)
        {
            var product = await _dao.Products.FindAsync(productId);

            if (product is null)
                throw new Exception("Товара не существует");

            return product.ProductPhotos.Select(p =>
            new PhotoResponse
            {
                PhotoId = p.Id,
                LowResPath = p.LowResPhotoPath,
                HighResPath = p.HighResPhotoPath
            })
            .ToArray();
        }

        public Task<PhotoResponse[]> GetAllProductPhotosAsync(int page)
        {
            if (page < 1)
                page = 1;

            return _dao.ProductPhotos
                .Skip((page-1)*PHOTOS_PER_PAGE)
                .Take(PHOTOS_PER_PAGE)
                .Select(p =>
                new PhotoResponse
                {
                    PhotoId = p.Id,
                    LowResPath = p.LowResPhotoPath,
                    HighResPath = p.HighResPhotoPath
                })
                .ToArrayAsync();
        }

        public Task<PhotoResponse[]> GetAllProductPhotosAsync()
        {
            return _dao.ProductPhotos
                .Select(p =>
                new PhotoResponse
                {
                    PhotoId = p.Id,
                    LowResPath = p.LowResPhotoPath,
                    HighResPath = p.HighResPhotoPath
                })
                .ToArrayAsync();
        }
    }
}
