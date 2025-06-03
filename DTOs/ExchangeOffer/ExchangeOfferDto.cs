using ElectroHub.DTOs.AnnouncementExchange;

namespace ElectroHub.DTOs.ExchangeOffer
{
    public class ExchangeOfferDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<AnnouncementExchangeDto> AnnouncementExchanges { get; set; }
    }
}
