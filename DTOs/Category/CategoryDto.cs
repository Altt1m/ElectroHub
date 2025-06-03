namespace ElectroHub.DTOs.Category
{
    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;

        public CategoryDto ParentCategory { get; set; }
        public List<CategoryDto> Children { get; set; } = new List<CategoryDto>();
    }
}
