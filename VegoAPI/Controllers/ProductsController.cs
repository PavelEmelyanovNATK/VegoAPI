using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Services.ProductsRepository;

namespace VegoAPI.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IProductsRepository productsRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productsRepository = productsRepository;
            _webHostEnvironment = webHostEnvironment;
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

        [HttpPost("get-with-filter")]
        public async Task<IActionResult> GetProductsWithFilter([FromBody] FilteredProductsRequest filteredProductsRequest)
        {
            return Ok(await _productsRepository.GetProductsWithFilterAsync(filteredProductsRequest));
        }

        [HttpGet("get-photo-low/{id}")]
        public async Task<FileResult> GetLowPhoto(Guid id)
        {
            return File(await _productsRepository.GetProductLowPhoto(id), "image/jpg");
        }

        [HttpGet("get-photo-high/{id}")]
        public async Task<FileResult> GetHighPhoto(Guid id)
        {
            return File(await _productsRepository.GetProductHighPhoto(id), "image/jpg");
        }
    }
}
