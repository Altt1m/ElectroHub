using ElectroHub.Models;

namespace ElectroHub.DTOs.Announcement
{
    public class AnnouncementUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
