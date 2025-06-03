using RentalManager.DTOs.User;

namespace RentalManager.DTOs.Tenant
{
    public class READTenantDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public string MobileNumber { get; set; } = string.Empty;

        public READUserDto User { get; set; } = null!;

        public int unitId { get; set; }

        public string? Unit { get; set; }

        public int TenantStatusId { get; set; }

        public string TenantStatus { get; set; } = string.Empty;
    }
}
