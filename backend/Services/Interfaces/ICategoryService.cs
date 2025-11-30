using backend.DTOs;
using backend.Models;

namespace backend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryById(int id);
    }
}
