namespace RentalManager.DTOs.User
{
    public class READUserDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public string MobileNumber { get; set; } = string.Empty;

        public string? AlternativeNumber { get; set; }

        public int? NationalId { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        public bool IsActive { get; set; } = false;

        public string GenderName { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public string UserStatus { get; set; } = string.Empty;

        public string PropertyName { get; set; } = string.Empty;
    }
}
