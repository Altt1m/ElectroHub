﻿namespace ElectroHub.DTOs.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int? ParentCategoryId { get; set; }
        public List<CategoryDto> Children { get; set; } = new List<CategoryDto>();
    }
}
