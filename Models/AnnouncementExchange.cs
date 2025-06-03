namespace ElectroHub.Models
{
    public class AnnouncementExchange
    {
        public int AnnouncementId { get; set; }
        public Announcement Announcement { get; set; } = null!;
        public int ExchangeOfferId { get; set; }
        public ExchangeOffer ExchangeOffer { get; set; } = null!;
    }
}
