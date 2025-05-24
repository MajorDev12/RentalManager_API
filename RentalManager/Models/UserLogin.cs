using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.Models
{
    public class UserLogin
    {
        public int Id { get; set; }

        public int FailedAttempts { get; set; } = 0;

        public bool Succeeded { get; set; }

        public string? IpAddress { get; set; }

        public string? UserToken { get; set; }

        public DateTime? TokenExpiry { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;



        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }

}
