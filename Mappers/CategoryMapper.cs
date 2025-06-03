using ElectroHub.DTOs.Announcement;
using ElectroHub.DTOs.Category;
using ElectroHub.Models;

namespace ElectroHub.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category categoryModel)
        {
            return new CategoryDto
            {
                Id = categoryModel.Id,
                Name = categoryModel.Name,
                ParentCategoryId = categoryModel.ParentCategoryId,
                Children = categoryModel.Children.Select(c => c.ToCategoryDto()).ToList()
            };
        }

        public static Category ToCategoryFromCreateDto(this CategoryCreateDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name,
                ParentCategoryId = categoryDto.ParentCategoryId,
            };
        }
    }
}
