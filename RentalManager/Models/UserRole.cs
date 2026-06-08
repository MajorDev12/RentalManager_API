using Microsoft.AspNetCore.Identity;

namespace RentalManager.Models
{
    public class UserRole
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int RoleId { get; set; }

        public int AccountId { get; set; } 
        public int? PropertyId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public User User { get; set; } = default!;
        public Role Role { get; set; } = default!;
    }
}
