namespace ElectroHub.Models
{
    public class ExchangeOffer
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<AnnouncementExchange> AnnouncementExchanges { get; set; } = new List<AnnouncementExchange>();
    }
}
