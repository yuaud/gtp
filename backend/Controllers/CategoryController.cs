using backend.DTOs;
using backend.Models;
using backend.Services;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto?>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("{category_id}/subcategories")]
        public async Task<ActionResult<IEnumerable<SubcategoryDto>>> GetSubcategoriesByCategory(int category_id)
        {
            var subcategoriesByCategory = await _categoryService.GetSubcategoryByCategory(category_id);
            return Ok(subcategoriesByCategory);
        }
    }
}
