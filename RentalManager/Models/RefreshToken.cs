namespace RentalManager.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTimeOffset ExpiresOn { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public string? CreatedByIp { get; set; }
        public string? RevokedByIp { get; set; }
        public bool Revoked { get; set; } = false;
        public DateTimeOffset? RevokedOn { get; set; }
        public string? ReplacedByToken { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}
