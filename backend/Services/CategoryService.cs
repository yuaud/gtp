using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(AppDbContext context, ILogger<CategoryService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            return await _context.Categories
                .Include(c => c.Subcategories)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<SubcategoryDto>> GetSubcategoryByCategory(int category_id)
        {
            try
            {
                var response = await _context.Subcategories
                    .Where(s => s.Category_id == category_id)
                    .Select(s => new SubcategoryDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        CategoryId = s.Category_id,
                        CategoryName = s.Category.Name
                    })
                    .ToListAsync();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Subcategories, Category: {category_id} icin fetch edilirken hata.", category_id);
                throw;
            }
        }
    }
}
