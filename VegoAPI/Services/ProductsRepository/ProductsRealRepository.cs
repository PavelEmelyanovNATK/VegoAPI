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

        public ProductsRealRepository(VegoCityServerDBContext dao)
        {
            _dao = dao;
        }

        public async Task AddProductAsync(AddProductRequest addProductRequest)
        {
            var product = new Product
            {
                Title = addProductRequest.Title,
                ProductTypeId = addProductRequest.ProductTypeId,
                Price = addProductRequest.Price,
                Description = addProductRequest.Description
            };

            await _dao.Products.AddAsync(product);
            await _dao.SaveChangesAsync();
        }

        public async Task LoadProductPhotoAsync(int productId, string lowResPhotoPath, string highResPhotoPath)
        {
            var product = await _dao.Products.FindAsync(productId);

            if (product is null)
                return;


            product.Photos.Add(new ProductPhoto
            {
                Guid = Guid.NewGuid(),
                LowResPhotoPath = lowResPhotoPath,
                HighResPhotoPath = highResPhotoPath
            });

            await _dao.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _dao.Products.FindAsync(id);

            if (product is null) return;

            _dao.ProductMainPhotos.Remove(product.ProductMainPhoto);
            var photos = product.Photos.ToArray();
            _dao.ProductPhotos.RemoveRange(photos);
            _dao.Products.Remove(product);
            await _dao.SaveChangesAsync();
        }

        public async Task EditProductInfoAsync(EditEntityRequest editProductRequest)
        {
            var product = await _dao.Products.FindAsync(editProductRequest.EntityId);

            if (product is null)
                return;

            editProductRequest.ChangedFields.GetValueOrDefault("Title")
            ?.Let(title => product.Title = title);

            editProductRequest.ChangedFields.GetValueOrDefault("CategoryId")
            ?.Let(productTypeId => product.ProductTypeId = Convert.ToInt32(productTypeId));

            editProductRequest.ChangedFields.GetValueOrDefault("Price")
            ?.Let(price => product.Price = Convert.ToDouble(price));

            editProductRequest.ChangedFields.GetValueOrDefault("Description")
            ?.Let(description => product.Description = description);

            await _dao.SaveChangesAsync();
        }

        public async Task<ProductShortResponse[]> GetAllProductsAsync()
            => await _dao.Products.Select(p => 
            new ProductShortResponse
            {
                Id = p.Id,
                Title = p.Title,
                Category = p.ProductType.Name,
                CategoryId = p.ProductTypeId,
                Price = p.Price,
                IsActive = true,
                ImagePath = p.ProductMainPhoto == null
                    ? ""
                    : p.ProductMainPhoto.Photo == null
                    ? ""
                    : @"http://26.254.208.125:3000/products/get-photo-low/" + p.ProductMainPhoto.PhotoId
            })
            .ToArrayAsync();

        public async Task<ProductDetailResponse> GetProductByIdAsync(int id)
        { 
            var product = await _dao.Products.FindAsync(id);

            if(product is null)
                return null;

            return new ProductDetailResponse
            {
                Id = product.Id,
                Title = product.Title,
                Category = product.ProductType.Name,
                CategoryId = product.ProductTypeId,
                Description = product.Description,
                Price = product.Price,
                IsActive = true,
                ImagePath = product.ProductMainPhoto == null
                    ? ""
                    : product.ProductMainPhoto.Photo == null
                    ? ""
                    : @"http://26.254.208.125:3000/products/get-photo-high/" + product.ProductMainPhoto.PhotoId
            };
        }

        public async Task<byte[]> GetProductLowPhoto(Guid photoId)
        {
            var photo = await _dao.ProductPhotos.FindAsync(photoId);

            var photoBytes = await System.IO.File.ReadAllBytesAsync(photo.LowResPhotoPath);

            return photoBytes;
        }

        public async Task<ProductShortResponse[]> GetProductsByCategoriesAsync(int[] categoriesIds)
        {
            IQueryable<Product> products;

            if (categoriesIds.Length > 0)
                products = _dao.Products.Where(p => categoriesIds.Contains(p.ProductTypeId));
            else
                products = _dao.Products;

            return await products.Select(p =>
            new ProductShortResponse
            {
                Id = p.Id,
                Title = p.Title,
                Category = p.ProductType.Name,
                CategoryId = p.ProductTypeId,
                Price = p.Price,
                IsActive = true,
                ImagePath = p.ProductMainPhoto == null
                    ? null
                    : p.ProductMainPhoto.Photo == null
                    ? null
                    : @"http://26.254.208.125:3000/products/get-photo-low/" + p.ProductMainPhoto.PhotoId
            })
            .ToArrayAsync();
        }

        public async Task<ProductShortResponse[]> GetProductsWithFilterAsync(FilteredProductsRequest filteredProductsRequest)
        {
            IQueryable<Product> products;

            if (filteredProductsRequest.CategoriesIds is not null && filteredProductsRequest.CategoriesIds.Length > 0)
                products = _dao.Products.Where(p => 
                filteredProductsRequest.CategoriesIds.Contains(p.ProductTypeId) && p.Title.Contains(filteredProductsRequest.Filter));
            else
                products = _dao.Products.Where(p => p.Title.Contains(filteredProductsRequest.Filter ?? ""));

            return await products
                .Select(p =>
                new ProductShortResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Category = p.ProductType.Name,
                    CategoryId = p.ProductTypeId,
                    Price = p.Price,
                    IsActive = true,
                    ImagePath =  p.ProductMainPhoto == null 
                    ? ""
                    : p.ProductMainPhoto.Photo == null
                    ? ""
                    : @"http://26.254.208.125:3000/products/get-photo-low/" + p.ProductMainPhoto.PhotoId
                })
                .ToArrayAsync();
        }

        public async Task SetProductMainPhotoAsync(int productId, Guid photoId)
        {
            var product = await _dao.Products.FindAsync(productId);

            if (product is null)
                return;

            product.ProductMainPhoto.PhotoId = photoId;

            await _dao.SaveChangesAsync();
        }

        public async Task LoadProductMainPhotoAsync(int productId, string lowResPhotoPath, string highResPhotoPath)
        {
            var product = await _dao.Products.FindAsync(productId);

            if (product is null)
                return;

            product.ProductMainPhoto = new ProductMainPhoto
            {
                Photo = new ProductPhoto
                {
                    Guid = Guid.NewGuid(),
                    LowResPhotoPath = lowResPhotoPath,
                    HighResPhotoPath = highResPhotoPath
                }
            };

            await _dao.SaveChangesAsync();
        }

        public async Task RemoveProductPhotoAsync(Guid photoId)
        {
            var photo = await _dao.ProductPhotos.FindAsync(photoId);
            if (photo is null)
                return;

            var mainPhotos = photo.ProductMainPhotos.ToArray();
            _dao.ProductMainPhotos.RemoveRange(mainPhotos);
            _dao.ProductPhotos.Remove(photo);
            await _dao.SaveChangesAsync();
        }

        public async Task<string[]> GetProductPhotosPaths(int productId)
        {
            var product = await _dao.Products.FindAsync(productId);
            if (product is null)
                return Array.Empty<string>();

            return product.Photos
                .Select(pl => pl.LowResPhotoPath)
                .Union(product.Photos.Select(ph => ph.HighResPhotoPath))
                .ToArray();
        }

        public async Task<byte[]> GetProductHighPhoto(Guid photoId)
        {
            var photo = await _dao.ProductPhotos.FindAsync(photoId);

            var photoBytes = await System.IO.File.ReadAllBytesAsync(photo.HighResPhotoPath);

            return photoBytes;
        }
    }
}
