using ElectroHub.DTOs.Announcement;
using ElectroHub.Helpers;
using ElectroHub.Models;
using System.Collections.Generic;

namespace ElectroHub.Interfaces
{
    public interface IAnnouncementService 
    { 
        Task<IEnumerable<Announcement>> GetAllAsync(AnnouncementQueryObject query);
        Task<Announcement?> GetByIdAsync(int id);
        Task<Announcement> CreateAsync(AnnouncementCreateDto dto, string userId);
        Task<Announcement?> UpdateAsync(int id, AnnouncementUpdateDto dto, string userId, bool isAdmin);
        Task<Announcement?> DeleteAsync(int id, string userId, bool isAdmin);
    }

}
