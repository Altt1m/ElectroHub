using ElectroHub.DTOs.Announcement;
using ElectroHub.DTOs.AnnouncementExchange;
using ElectroHub.DTOs.Category;
using ElectroHub.Models;

namespace ElectroHub.Mappers
{
    public static class AnnouncementExchangeMapper
    {
        public static AnnouncementExchangeDto ToAnnouncementExchangeDto(this AnnouncementExchange announcementExchange)
        {
            return new AnnouncementExchangeDto
            {
                AnnouncementId = announcementExchange.AnnouncementId,
                ExchangeOfferId = announcementExchange.ExchangeOfferId
            };
        }
    }
}
