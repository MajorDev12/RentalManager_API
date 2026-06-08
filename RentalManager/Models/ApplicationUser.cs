using Microsoft.AspNetCore.Identity;

namespace RentalManager.Models
{
    public class ApplicationUser : IdentityUser<int>
    {

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int AccountId { get; set; }

        public User? User { get; set; } 
    }
}
