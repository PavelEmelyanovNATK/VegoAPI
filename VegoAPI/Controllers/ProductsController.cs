using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VegoAPI.Services.ProductsRepository;

namespace VegoAPI.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productsRepository.GetAllProductsAsync());
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            return Ok(await _productsRepository.GetProductByIdAsync(id));
        }
    }
}
