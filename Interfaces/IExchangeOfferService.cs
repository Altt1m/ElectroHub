using ElectroHub.DTOs.Category;
using ElectroHub.DTOs.ExchangeOffer;
using ElectroHub.Models;

namespace ElectroHub.Interfaces
{
    public interface IExchangeOfferService
    {
        Task<IEnumerable<ExchangeOffer>> GetAllAsync();
        Task<ExchangeOffer?> GetByIdAsync(int id);
        Task<ExchangeOffer> CreateExchangeOfferAsync(ExchangeOfferCreateDto dto, string userId);
        Task<ExchangeOffer?> UpdateAsync(int id, ExchangeOfferDto categoryDto);
        Task<ExchangeOffer?> DeleteAsync(int id);
    }
}
