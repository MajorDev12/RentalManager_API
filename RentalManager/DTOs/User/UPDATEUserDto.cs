namespace RentalManager.DTOs.User
{
    public class UPDATEUserDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? UserName { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string? AlternativeNumber { get; set; }

        public int? NationalId { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        public bool IsActive { get; set; } = false;

        public int GenderId { get; set; }

        public int UserStatusId { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
