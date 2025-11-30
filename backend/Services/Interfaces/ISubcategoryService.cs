using backend.DTOs;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<SubcategoryDto>> GetAllSubcategoriesAsync();
        Task<SubcategoryDto?> GetSubcategoryById(int id);
    }
}
