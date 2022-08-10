using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Models.ResponseModels;
using VegoAPI.Services.ProductsRepository;
using VegoAPI.Utils;

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
            try
            {
                return Ok(await _productsRepository.GetAllProductsAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                return Ok(await _productsRepository.GetProductByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

        }

        [HttpGet("get-short/{id}")]
        public async Task<IActionResult> GetShortProductById(Guid id)
        {
            try
            {
                var productDetails = await _productsRepository.GetProductByIdAsync(id);

                var mainPhoto = productDetails.Photos.FirstOrDefault(p => p.PhotoId == productDetails.MainPhotoId)?.LowResPath;

                var productShort = new ProductShortResponse
                {
                    Id = productDetails.Id,
                    Title = productDetails.Title,
                    Category = productDetails.Category,
                    CategoryId = productDetails.CategoryId,
                    Price = productDetails.Price,
                    IsActive = productDetails.IsActive,
                    ImagePath = mainPhoto
                };
                return Ok(productShort);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

        }

        [HttpPost("get-with-filter")]
        public async Task<IActionResult> GetProductsWithFilter([FromBody] FilteredProductsRequest filteredProductsRequest)
        {
            try
            {
                return Ok(await _productsRepository.GetProductsWithFilterAsync(filteredProductsRequest));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

        }

        [HttpGet("get-all-product-photos/{productId}")]
        public async Task<IActionResult> GetAllProductPhotos(Guid productId)
        {
            try
            {
                return Ok(await _productsRepository.GetAllProductPhotosAsync(productId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpGet("get-all-photos-page/{page}")]
        public async Task<IActionResult> GetAllProductPhotos(int page)
        {
            try
            {
                return Ok(await _productsRepository.GetAllProductPhotosAsync(page));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());

            }
        }

        [HttpGet("get-all-photos")]
        public async Task<IActionResult> GetAllProductPhotos()
        {
            try
            {
                return Ok(await _productsRepository.GetAllProductPhotosAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());

            }
        }
    }
}
