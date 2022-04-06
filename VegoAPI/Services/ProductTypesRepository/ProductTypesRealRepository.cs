using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;
using VegoAPI.Utils;
using VegoAPI.VegoAPI.Models.DBEntities;
using VegoAPI.VegoAPI.Services.DBContext;

namespace VegoAPI.Services.ProductTypesRepository
{
    public class ProductTypesRealRepository : IProductTypesRepository
    {
        private readonly VegoCityServerDBContext _dao;

        public ProductTypesRealRepository(VegoCityServerDBContext dao)
        {
            _dao = dao;
        }

        public async Task AddProductTypeAsync(AddProductTypeRequest addProductRequest)
        {
            var productType = new ProductType
            {
                Name = addProductRequest.Name,
            };

            await _dao.ProductTypes.AddAsync(productType);
            await _dao.SaveChangesAsync();
        }

        public async Task DeleteProductTypeAsync(int productTypeId)
        {
            var productType = await _dao.ProductTypes.FindAsync(productTypeId);

            if(productType is null)
                return;

            _dao.ProductTypes.Remove(productType);
            await _dao.SaveChangesAsync();
        }

        public async Task EditProductTypeAsync(EditEntityRequest editProductTypeRequest)
        {
            var productType = await _dao.ProductTypes.FindAsync(editProductTypeRequest.EntityId);

            if (productType is null)
                return;

            editProductTypeRequest.ChangedFields["name"]
            ?.Let(name => productType.Name = name);

            await _dao.SaveChangesAsync();
        }

        public async Task<ProductTypeResponse[]> GetAllProductTypesAsync()
            => await _dao.ProductTypes
            .Select(pt =>
            new ProductTypeResponse
            {
                Id = pt.Id,
                Name = pt.Name
            })
            .ToArrayAsync();

        public async Task<ProductTypeResponse> GetProductTypeByIdAsync(int id)
        {
            var productType = await _dao.ProductTypes.FindAsync(id);

            return new ProductTypeResponse 
            { 
                Name = productType.Name 
            };
        }
    }
}
