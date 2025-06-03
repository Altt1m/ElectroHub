using ElectroHub.DTOs.Category;
using ElectroHub.DTOs.ExchangeOffer;
using ElectroHub.Interfaces;
using ElectroHub.Mappers;
using ElectroHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectroHub.Controllers
{
    [Route("api/exchange-offer")]
    [ApiController]
    public class ExchangeOfferController : ControllerBase
    {
        private readonly IExchangeOfferService _exchangeOfferService;
        public ExchangeOfferController(IExchangeOfferService exchangeOfferService)
        {
            _exchangeOfferService = exchangeOfferService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var offers = await _exchangeOfferService.GetAllAsync();

            var offerDto = offers.Select(s => s.ToExchangeOfferDto()).ToList();

            return Ok(offerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ExchangeOfferCreateDto offerCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Отримуємо ID користувача з JWT-токена (string)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token.");

            try
            {
                var createdOffer = await _exchangeOfferService.CreateExchangeOfferAsync(
                    offerCreateDto, userId);

                return CreatedAtAction(nameof(GetById),
                    new { id = createdOffer.Id },
                    createdOffer.ToExchangeOfferDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var offer = await _exchangeOfferService.GetByIdAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            return Ok(offer.ToExchangeOfferDto());
        }
    }
}
