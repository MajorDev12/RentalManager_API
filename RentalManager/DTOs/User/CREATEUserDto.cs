using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.User
{
    public class CREATEUserDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required]
        public string MobileNumber { get; set; } = string.Empty;

        public string? AlternativeNumber { get; set; }

        public string PasswordHash { get; set; } = string.Empty;

        public DateTime? LastPasswordChange { get; set; }

        public int? NationalId { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        public bool IsActive { get; set; } = false;

        public int GenderId { get; set; }
        public int RoleId { get; set; }
        public int UserStatusId { get; set; }
        public int PropertyId { get; set; }
    }
}
