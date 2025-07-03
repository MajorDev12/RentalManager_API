using RentalManager.DTOs.User;

namespace RentalManager.DTOs.Tenant
{
    public class UPDATETenantDto
    {

        public string FullName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public string MobileNumber { get; set; } = string.Empty;

        public UPDATEUserDto User { get; set; } = null!;

        public int Status { get; set; }

    }
}
