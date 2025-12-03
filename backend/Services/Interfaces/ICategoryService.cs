using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryById(int id);
        Task<IEnumerable<SubcategoryDto>> GetSubcategoryByCategory(int category_id);
    }
}
