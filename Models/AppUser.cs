using Microsoft.AspNetCore.Identity;

namespace ElectroHub.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsBlocked { get; set; } = false;
        public List<Announcement> Announcements { get; set; } = new List<Announcement>();
    }
}
