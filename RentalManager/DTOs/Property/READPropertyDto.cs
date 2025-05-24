namespace RentalManager.DTOs.Property
{
    public class READPropertyDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

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
    }
}
