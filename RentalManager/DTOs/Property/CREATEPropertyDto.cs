using RentalManager.DTOs.UtilityBill;
using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Property
{
    public class CREATEPropertyDto
    {
        public required string Name { get; set; }

        public string Country { get; set; } = string.Empty;

        public string County { get; set; } = string.Empty;

        public string Area { get; set; } = string.Empty;

        public string PhysicalAddress { get; set; } = string.Empty;

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int Floor { get; set; }
        public string? Notes { get; set; }

        public string EmailAddress { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public int PropertyTypeId { get; set; }

        public List<CREATEPropertyUtilityDto>? Utilities { get; set; }
    }
}
