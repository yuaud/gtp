using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace backend.Services
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly AppDbContext _context;

        public SubcategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubcategoryDto>> GetAllSubcategoriesAsync()
        {
            return await _context.Subcategories
                .Include(s => s.Category)
                .Select(s => new SubcategoryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.Category_id,
                    CategoryName = s.Category.Name
                })
                .ToListAsync();
        }

        public async Task<SubcategoryDto?> GetSubcategoryById(int id)
        {
            return await _context.Subcategories
                .Include(s => s.Category)
                .Select(s => new SubcategoryDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    CategoryId = s.Category_id,
                    CategoryName = s.Category.Name
                })
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
