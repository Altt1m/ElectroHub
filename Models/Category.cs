namespace ElectroHub.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Category? ParentCategory { get; set; } = null;
        public int? ParentCategoryId { get; set; }

        public List<Category> Children { get; set; } = new List<Category>();

        public List<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
}
