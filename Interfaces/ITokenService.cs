using ElectroHub.Models;

namespace ElectroHub.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(AppUser user, List<string> roles);
    }
}
