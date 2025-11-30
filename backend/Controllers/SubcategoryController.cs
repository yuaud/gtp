using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubcategoryDto>>> GetSubcategories()
        {
            var subcategories = await _subcategoryService.GetAllSubcategoriesAsync();
            return Ok(subcategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubcategoryDto?>> GetSubcategory(int id)
        {
            var subcategory = await _subcategoryService.GetSubcategoryById(id);
            if (subcategory == null) return NotFound();
            return Ok(subcategory);
        }
    }
}
