using ElectroHub.DataAccess;
using ElectroHub.DTOs.Category;
using ElectroHub.DTOs.ExchangeOffer;
using ElectroHub.Interfaces;
using ElectroHub.Mappers;
using ElectroHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectroHub.Services
{
    public class ExchangeOfferService : IExchangeOfferService
    {
        private readonly ApplicationDbContext _context;
        public ExchangeOfferService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ExchangeOffer> CreateExchangeOfferAsync(ExchangeOfferCreateDto dto, string userId)
        {
            // Перевірка існування оголошень
            var sourceAnnouncement = await _context.Announcements
                .FirstOrDefaultAsync(a => a.Id == dto.SourceAnnouncementId);
            if (sourceAnnouncement == null)
            {
                throw new ArgumentException($"Announcement with ID {dto.SourceAnnouncementId} does not exist.");
            }

            var targetAnnouncement = await _context.Announcements
                .FirstOrDefaultAsync(a => a.Id == dto.TargetAnnouncementId);
            if (targetAnnouncement == null)
            {
                throw new ArgumentException($"Announcement with ID {dto.TargetAnnouncementId} does not exist.");
            }

            // Перевірка, чи SourceAnnouncement належить користувачу
            if (sourceAnnouncement.AppUserId != userId)
            {
                throw new UnauthorizedAccessException("You are not the owner of the source announcement.");
            }

            // Створення ExchangeOffer
            var offer = dto.ToExchangeOfferFromDto();
            _context.ExchangeOffers.Add(offer);
            await _context.SaveChangesAsync(); // Зберігаємо, щоб отримати Offer.Id

            // Перевірка кількості записів у AnnouncementExchange
            var existingExchanges = await _context.AnnouncementExchanges
                .CountAsync(ae => ae.ExchangeOfferId == offer.Id);
            if (existingExchanges >= 2)
            {
                throw new ArgumentException("Exchange offer already has two associated announcements.");
            }

            // Створення двох записів у AnnouncementExchange
            var exchange1 = new AnnouncementExchange
            {
                AnnouncementId = dto.SourceAnnouncementId,
                ExchangeOfferId = offer.Id
            };
            var exchange2 = new AnnouncementExchange
            {
                AnnouncementId = dto.TargetAnnouncementId,
                ExchangeOfferId = offer.Id
            };

            _context.AnnouncementExchanges.AddRange(exchange1, exchange2);
            await _context.SaveChangesAsync();

            // Завантажуємо пов’язані дані для відповіді
            return await _context.ExchangeOffers
                .Include(o => o.AnnouncementExchanges)
                .FirstAsync(o => o.Id == offer.Id);
        }

        public async Task<ExchangeOffer?> GetByIdAsync(int id)
        {
            return await _context.ExchangeOffers
                .Include(o => o.AnnouncementExchanges.Where(o => o.ExchangeOfferId == id))
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<ExchangeOffer?> DeleteAsync(int id)
        {
            var exchangeOfferModel = await _context.ExchangeOffers.FirstOrDefaultAsync(x => x.Id == id);

            if (exchangeOfferModel == null)
            {
                return null;
            }

            _context.ExchangeOffers.Remove(exchangeOfferModel);

            await _context.SaveChangesAsync();

            return exchangeOfferModel;
        }

        public async Task<IEnumerable<ExchangeOffer>> GetAllAsync()
        {
            return await _context.ExchangeOffers
                .Include(o => o.AnnouncementExchanges)
                .ToListAsync();
        }

        public async Task<ExchangeOffer?> UpdateAsync(int id, ExchangeOfferDto exchangeOfferDto)
        {
            var existingExchangeOffer = await _context.ExchangeOffers.FirstOrDefaultAsync(x => x.Id == id);

            if (existingExchangeOffer == null)
            {
                return null;
            }

            existingExchangeOffer.CreatedAt = exchangeOfferDto.CreatedAt;

            await _context.SaveChangesAsync();

            return existingExchangeOffer;
        }
    }
}
