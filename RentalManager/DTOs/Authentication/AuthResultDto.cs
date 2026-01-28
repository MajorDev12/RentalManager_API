namespace RentalManager.DTOs.Authentication
{
    public class AuthResultDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTimeOffset RefreshTokenExpiry { get; set; }
    }
}
