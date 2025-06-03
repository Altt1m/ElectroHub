using ElectroHub.DTOs.ExchangeOffer;
using ElectroHub.Models;

namespace ElectroHub.Mappers
{
    public static class ExchangeOfferMapper
    {
        public static ExchangeOfferDto ToExchangeOfferDto(this ExchangeOffer exchangeOfferModel)
        {
            return new ExchangeOfferDto
            {
                Id = exchangeOfferModel.Id,
                CreatedAt = exchangeOfferModel.CreatedAt,
                AnnouncementExchanges = exchangeOfferModel.AnnouncementExchanges.Select(e => e.ToAnnouncementExchangeDto()).ToList()
            };
        }

        public static ExchangeOffer ToExchangeOfferFromDto(this ExchangeOfferCreateDto exchangeOfferDto)
        {
            return new ExchangeOffer
            {
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
