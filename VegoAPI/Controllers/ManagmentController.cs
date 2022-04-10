using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Services.ProductsRepository;
using VegoAPI.Services.ProductTypesRepository;
using VegoAPI.Utils;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Hosting;
using VegoAPI.Services.PhotosHandler;

namespace VegoAPI.Controllers
{
    
    [Route("managment")]
    public class ManagmentController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductTypesRepository _productTypesRepository;
        private readonly IPhotosHandler _photosHandler;

        public ManagmentController(
            IProductsRepository productsRepository,
            IProductTypesRepository productTypesRepository, 
            IPhotosHandler photosHandler)
        {
            _productsRepository = productsRepository;
            _productTypesRepository = productTypesRepository;
            _photosHandler = photosHandler;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest addProductRequest)
        {
            try
            {
                await _productsRepository.AddProductAsync(addProductRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

            return Ok();
        }

        [HttpPut("edit-product-info")]
        public async Task<IActionResult> EditProductInfo([FromBody] EditEntityRequest editProductRequest)
        {
            try
            {
                await _productsRepository.EditProductInfoAsync(editProductRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

            return Ok();
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var productPhotosPaths = await _productsRepository.GetProductPhotosPaths(id);

                foreach (var path in productPhotosPaths)
                    await _photosHandler.DeleteProductPhoto(path);

                await _productsRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

            return Ok();
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> AddProductType([FromBody] AddProductTypeRequest addProductTypeRequest)
        {
            try
            {
                await _productTypesRepository.AddProductTypeAsync(addProductTypeRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

            return Ok();
        }

        [HttpPut("edit-category")]
        public async Task<IActionResult> EditProductType([FromBody] EditEntityRequest editProductTypeRequest)
        {
            try
            {
                await _productTypesRepository.EditProductTypeAsync(editProductTypeRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

            return Ok();
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            try
            {
                await _productTypesRepository.DeleteProductTypeAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

            return Ok();
        }

        [HttpPost("load-product-main-photo")]
        public async Task<IActionResult> LoadProductMainPhoto([FromForm] LoadProductImageRequest loadProductImageRequest)
        {
            if (await _productsRepository.GetProductByIdAsync(loadProductImageRequest.ProductId) is null)
                return BadRequest("Товара не существует");

            if (loadProductImageRequest.ImageFile.Length == 0)
            {
                return BadRequest();
            }

            var paths = await _photosHandler.SaveProductPhoto(loadProductImageRequest);

            await _productsRepository.LoadProductMainPhotoAsync(loadProductImageRequest.ProductId, paths.Item1, paths.Item2);

            return Ok();
        }

        [HttpPost("load-product-photo")]
        public async Task<IActionResult> LoadProductPhoto([FromForm] LoadProductImageRequest loadProductImageRequest)
        {
            if (await _productsRepository.GetProductByIdAsync(loadProductImageRequest.ProductId) is null)
                return BadRequest("Товара не существует");

            if (loadProductImageRequest.ImageFile.Length == 0)
            {
                return BadRequest();
            }

            var paths = await _photosHandler.SaveProductPhoto(loadProductImageRequest);

            await _productsRepository.LoadProductPhotoAsync(loadProductImageRequest.ProductId, paths.Item1, paths.Item2);

            return Ok();
        }
    }
}
