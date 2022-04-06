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

        public async Task DeleteProductAsync(int id)
        {
            var product = await _dao.Products.FindAsync(id);

            if (product is null) return;

            _dao.Products.Remove(product);
            await _dao.SaveChangesAsync();
        }

        public async Task EditProductInfoAsync(EditEntityRequest editProductRequest)
        {
            var product = await _dao.Products.FindAsync(editProductRequest.EntityId);

            if (product is null)
                return;

            editProductRequest.ChangedFields["title"]
            ?.Let(title => product.Title = title);

            editProductRequest.ChangedFields["productTypeId"]
            ?.Let(productTypeId => product.ProductTypeId = Convert.ToInt32(productTypeId));

            editProductRequest.ChangedFields["price"]
            ?.Let(price => product.Price = Convert.ToDouble(price));

            editProductRequest.ChangedFields["description"]
            ?.Let(description => product.Description = description);

            await _dao.SaveChangesAsync();
        }

        public async Task<ProductShortResponse[]> GetAllProductsAsync()
            => await _dao.Products.Select(p => 
            new ProductShortResponse
            {
                Id = p.Id,
                Title = p.Title,
                ProductType = p.ProductType.Name,
                Price = p.Price,
                IsActive = true
            })
            .ToArrayAsync();

        public async Task<ProductShortResponse> GetProductByIdAsync(int id)
        { 
            var product = await _dao.Products.FindAsync(id);

            return new ProductShortResponse
            {
                Id = product.Id,
                Title = product.Title,
                ProductType = product.ProductType.Name,
                Price = product.Price,
                IsActive = true
            };
        }
    }
}
