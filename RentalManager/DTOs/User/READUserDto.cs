namespace RentalManager.DTOs.User
{
    public class READUserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public string MobileNumber { get; set; } = string.Empty;

        public string? AlternativeNumber { get; set; }

        public int? NationalId { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        public bool IsActive { get; set; } = false;

        public int GenderId { get; set; }

        public string GenderName { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public string UserStatus { get; set; } = string.Empty;

        public int propertyId { get; set; }

        public string PropertyName { get; set; } = string.Empty;
    }
}
