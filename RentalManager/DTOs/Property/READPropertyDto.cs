namespace RentalManager.DTOs.Property
{
    public class READPropertyDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string County { get; set; } = null!;

        public string Area { get; set; } = null!;

        public string PhysicalAddress { get; set; } = null!;

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int Floor { get; set; }

        public string EmailAddress { get; set; } = null!;

        public string MobileNumber { get; set; } = null!;

        public string? Notes { get; set; }

        public int PropertyTypeId { get; set; }
        public string PropertyTypeName { get; set; } = null!;
    }
}
