using RentalManager.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Tenant
{
    public class CREATETenantDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        [Required]
        public string MobileNumber { get; set; } = string.Empty;

        public CREATEUserDto User { get; set; } = null!;

        public int? UnitId { get; set; }

        public int Status { get; set; }
    }
}
