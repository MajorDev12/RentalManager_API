using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentalManager.Models
{
    public class User : AuditableEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public string MobileNumber { get; set; } = string.Empty;

        public string? AlternativeNumber { get; set; }

        public string PasswordHash { get; set; } = string.Empty;
        
        public DateTime? LastPasswordChange { get; set; }

        public int? NationalId { get; set; }

        public string? ProfilePhotoUrl { get; set; }

        public bool IsActive { get; set; } = false;



        public int GenderId { get; set; }
        public SystemCodeItem Gender { get; set; } = null!;

        public int UserStatusId { get; set; }
        public SystemCodeItem UserStatus { get; set; } = null!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property{ get; set; } = null!;

    }
}
