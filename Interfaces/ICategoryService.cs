using ElectroHub.DTOs.Category;
using ElectroHub.Models;

namespace ElectroHub.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category categoryModel);
        Task<Category?> UpdateAsync(int id, CategoryDto categoryDto);
        Task<Category?> DeleteAsync(int id);
    }
}
