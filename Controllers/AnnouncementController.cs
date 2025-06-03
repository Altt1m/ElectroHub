using ElectroHub.DataAccess;
using ElectroHub.DTOs.Announcement;
using ElectroHub.Helpers;
using ElectroHub.Interfaces;
using ElectroHub.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectroHub.Controllers
{
    [Route("api/announcement")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] AnnouncementQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var announcements = await _announcementService.GetAllAsync(query);
            var announcementDto = announcements.Select(s => s.ToAnnouncementDto()).ToList();

            return Ok(announcementDto);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var announcement = await _announcementService.GetByIdAsync(id);
            if (announcement == null)
                return NotFound();

            return Ok(announcement.ToAnnouncementDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([FromBody] AnnouncementCreateDto announcementDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token.");

            try
            {
                var announcementModel = announcementDto.ToAnnouncementFromCreateDto();
                announcementModel.AppUserId = userId;
                var createdAnnouncement = await _announcementService.CreateAsync(announcementDto, userId);
                return CreatedAtAction(nameof(GetById), new { id = createdAnnouncement.Id }, announcementModel.ToAnnouncementDto());
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message }); // Повідомлення про неіснуючу категорію
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AnnouncementUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token.");
            var isAdmin = User.IsInRole("Admin");

            try
            {
                var announcementModel = await _announcementService.UpdateAsync(id, updateDto, userId, isAdmin);
                if (announcementModel == null)
                    return NotFound();

                return Ok(announcementModel.ToAnnouncementDto());
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? throw new UnauthorizedAccessException("User ID not found in token.");
            var isAdmin = User.IsInRole("Admin");

            try
            {
                var announcementModel = await _announcementService.DeleteAsync(id, userId, isAdmin);
                if (announcementModel == null)
                    return NotFound();

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }

}
