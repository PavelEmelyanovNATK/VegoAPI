using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VegoAPI.Models.RequestModels;
using VegoAPI.Services.ProductsRepository;
using VegoAPI.Services.ProductTypesRepository;
using VegoAPI.Utils;

namespace VegoAPI.Controllers
{
    [ApiController]
    [Route("managment")]
    public class ManagmentController : ControllerBase
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IProductTypesRepository _productTypesRepository;

        public ManagmentController(
            IProductsRepository productsRepository, 
            IProductTypesRepository productTypesRepository)
        {
            _productsRepository = productsRepository;
            _productTypesRepository = productTypesRepository;
        }

        [HttpPost("add-product-info")]
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
                await _productsRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.WrapToArray());
            }

            return Ok();
        }

        [HttpPost("add-product-type")]
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

        [HttpPut("edit-product-type")]
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

        [HttpDelete("delete-product-type/{id}")]
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
    }
}
