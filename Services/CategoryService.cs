using ElectroHub.DataAccess;
using ElectroHub.DTOs.Category;
using ElectroHub.Interfaces;
using ElectroHub.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectroHub.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddAsync(Category categoryModel)
        {
            await _context.Categories.AddAsync(categoryModel);

            await _context.SaveChangesAsync();

            return categoryModel;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var categoryModel = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (categoryModel == null)
            {
                return null;
            }

            _context.Categories.Remove(categoryModel);

            await _context.SaveChangesAsync();

            return categoryModel;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(c => c.Children).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Category?> UpdateAsync(int id, CategoryDto categoryDto)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = categoryDto.Name;

            await _context.SaveChangesAsync();

            return existingCategory;
        }
    }
}
