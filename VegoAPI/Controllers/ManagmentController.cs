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
using VegoAPI.Models.ResponseModels;
using VegoAPI.Services.OrdersRepository;

namespace VegoAPI.Controllers
{
    
    [Route("managment")]
    public class ManagmentController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductTypesRepository _productTypesRepository;
        private readonly IOrdersRepository _ordersRepository;

        public ManagmentController(
            IProductsRepository productsRepository,
            IProductTypesRepository productTypesRepository,
            IOrdersRepository ordersRepository)
        {
            _productsRepository = productsRepository;
            _productTypesRepository = productTypesRepository;
            _ordersRepository = ordersRepository;
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest addProductRequest)
        {
            try
            {
                var id = await _productsRepository.AddProductAsync(addProductRequest);

                return Ok(new ProductAddedResponse { ProductId = id});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpPut("edit-product-info")]
        public async Task<IActionResult> EditProductInfo([FromBody] EditEntityWithGuidRequest editProductRequest)
        {
            try
            {
                await _productsRepository.EditProductInfoAsync(editProductRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productsRepository.DeleteProductAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> AddProductType([FromBody] AddProductTypeRequest addProductTypeRequest)
        {
            try
            {
                await _productTypesRepository.AddProductTypeAsync(addProductTypeRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpPut("edit-category")]
        public async Task<IActionResult> EditProductType([FromBody] EditEntityWithIntIdRequest editProductTypeRequest)
        {
            try
            {
                await _productTypesRepository.EditProductTypeAsync(editProductTypeRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpDelete("delete-category/{id}")]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            try
            {
                await _productTypesRepository.DeleteProductTypeAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpPost("add-product-photo")]
        public async Task<IActionResult> AddProductPhoto([FromBody] AddProductPhotoRequest addProductImageRequest)
        {
            try
            {
                var id = await _productsRepository.AddProductPhotoAsync(addProductImageRequest);

                return Ok(new PhotoAddedResponse { PhotoId = id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpPost("set-product-main-photo")]
        public async Task<IActionResult> SetProductMainPhoto([FromBody] ProductToPhotoRequest setProductMainPhotoRequest)
        {
            try
            {
                await _productsRepository.SetProductMainPhotoAsync(setProductMainPhotoRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpDelete("delete-product-photo/{photoId}")]
        public async Task<IActionResult> DeleteProductPhoto(Guid photoId)
        {
            try
            {
                await _productsRepository.DeleteProductPhotoAsync(photoId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpGet("get-orders/{page}/{ordersPerPage}")]
        public async Task<IActionResult> GetOrders(int page, int ordersPerPage)
        {
            if (page < 1) page = 1;
            if (ordersPerPage < 1) ordersPerPage = 30;

            try
            {
                return Ok(await _ordersRepository.GetAllOrdersAsync(page, ordersPerPage));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }
        
        [HttpGet("get-order-details/{id}")]
        public async Task<IActionResult> GetOrderDetails(Guid id)
        {
            try
            {
                return Ok(await _ordersRepository.GetOrderByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        [HttpPost("change-order-status")]
        public async Task<IActionResult> ChangeOrderStatus(ChangeOrderStatusRequest changeOrderStatusRequest)
        {
            try
            {
                await _ordersRepository.ChangeOrderStatusAsync(changeOrderStatusRequest);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }
        }

        ///// <summary>
        ///// Depricated.
        ///// </summary>
        ///// <param name="loadProductImageRequest"></param>
        ///// <returns></returns>
        //[HttpPost("load-product-main-photo")]
        //public async Task<IActionResult> LoadProductMainPhoto([FromForm] LoadProductImageFileRequest loadProductImageRequest)
        //{
        //    if (await _productsRepository.GetProductByIdAsync(loadProductImageRequest.ProductId) is null)
        //        return BadRequest("Товара не существует");
        //
        //    if (loadProductImageRequest.ImageFile.Length == 0)
        //    {
        //        return BadRequest();
        //    }
        //
        //    var paths = await _photosHandler.SaveProductPhoto(loadProductImageRequest);
        //
        //    //await _productsRepository.LoadProductMainPhotoAsync(loadProductImageRequest.ProductId, paths.Item1, paths.Item2);
        //
        //    return Ok();
        //}
    }
}
