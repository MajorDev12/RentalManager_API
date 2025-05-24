using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.Models
{
    public class Landlord
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = new();

        public string FullName { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

    }

}
