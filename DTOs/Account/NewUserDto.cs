namespace ElectroHub.DTOs.Account
{
    public class NewUserDto
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string AccessToken { get; set; }
    }
}
