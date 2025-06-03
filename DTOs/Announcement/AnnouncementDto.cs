using ElectroHub.DTOs.Category;
using ElectroHub.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroHub.DTOs.Announcement
{
    public class AnnouncementDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public required string AppUserId { get; set; }
        public int CategoryId { get; set; }
    }
}
