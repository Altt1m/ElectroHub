using System.ComponentModel.DataAnnotations.Schema;

namespace ElectroHub.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AppUser AppUser { get; set; } = null!;
        public required string AppUserId { get; set; }
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; }


        public List<AnnouncementExchange> AnnouncementExchanges { get; set; } = new List<AnnouncementExchange>();

    }
}
