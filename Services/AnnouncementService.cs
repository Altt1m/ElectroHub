using ElectroHub.DataAccess;
using ElectroHub.DTOs.Announcement;
using ElectroHub.Helpers;
using ElectroHub.Interfaces;
using ElectroHub.Mappers;
using ElectroHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ElectroHub.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync(AnnouncementQueryObject query)
        {
            var announcements = _context.Announcements.Include(a => a.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                announcements = announcements.Where(s => s.Title.Contains(query.Title));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    announcements = query.IsDescending ? announcements.OrderByDescending(s => s.Price) : announcements.OrderBy(s => s.Price);
                }

                if (query.SortBy.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase))
                {
                    announcements = query.IsDescending ? announcements.OrderByDescending(s => s.CreatedAt) : announcements.OrderBy(s => s.CreatedAt);
                }
            }

            if (query.WasUsed)
            {
                announcements = announcements.Where(s => s.Status == "Used");
            }

            if (query.ForExchange)
            {
                announcements = announcements.Where(s => s.OperationType == "Exchange");
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await announcements.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Announcement?> GetByIdAsync(int id)
        {
            return await _context.Announcements
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Announcement> CreateAsync(AnnouncementCreateDto dto, string userId)
        {
            var announcement = dto.ToAnnouncementFromCreateDto();
            announcement.AppUserId = userId;

            // Перевірка існування категорії
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == announcement.CategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException("Category with the specified ID does not exist.");
            }

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return announcement;
        }

        public async Task<Announcement?> UpdateAsync(int id, AnnouncementUpdateDto dto, string userId, bool isAdmin)
        {
            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(a => a.Id == id);

            if (announcement == null)
                return null;

            // Перевірка власника або ролі Admin
            if (announcement.AppUserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("You are not authorized to update this announcement.");

            announcement.Title = dto.Title;
            announcement.Description = dto.Description;
            announcement.Price = dto.Price;
            announcement.Status = dto.Status;
            announcement.OperationType = dto.OperationType;
            announcement.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
            return announcement;
        }

        public async Task<Announcement?> DeleteAsync(int id, string userId, bool isAdmin)
        {
            var announcement = await _context.Announcements
                .FirstOrDefaultAsync(a => a.Id == id);

            if (announcement == null)
                return null;

            // Перевірка власника або ролі Admin
            if (announcement.AppUserId != userId && !isAdmin)
                throw new UnauthorizedAccessException("You are not authorized to delete this announcement.");

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();
            return announcement;
        }
    }

}