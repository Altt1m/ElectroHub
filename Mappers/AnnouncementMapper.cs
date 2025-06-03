using ElectroHub.DTOs.Announcement;
using ElectroHub.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;

namespace ElectroHub.Mappers
{
    public static class AnnouncementMapper
    {
        public static AnnouncementDto ToAnnouncementDto(this Announcement announcementModel)
        {
            return new AnnouncementDto
            {
                Id = announcementModel.Id,
                Title = announcementModel.Title,
                Description = announcementModel.Description,
                Price = announcementModel.Price,
                Status = announcementModel.Status,
                OperationType = announcementModel.OperationType,
                CreatedAt = DateTime.UtcNow,
                AppUserId = announcementModel.AppUserId,
                CategoryId = announcementModel.CategoryId
            };
        }

        public static Announcement ToAnnouncementFromCreateDto(this AnnouncementCreateDto announcementDto)
        {
            return new Announcement
            {
                Title = announcementDto.Title,
                Description = announcementDto.Description,
                Price = announcementDto.Price,
                Status = announcementDto.Status,
                OperationType = announcementDto.OperationType,
                CreatedAt = DateTime.UtcNow,
                CategoryId = announcementDto.CategoryId,
                AppUserId = string.Empty
            };

        }
    }
}
