using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VegoAPI.Services.ProductTypesRepository;

namespace VegoAPI.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IProductTypesRepository _categoriesRepository;

        public CategoriesController(IProductTypesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoriesRepository.GetAllProductTypesAsync());
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            return Ok(await _categoriesRepository.GetProductTypeByIdAsync(id));
        }
    }
}
