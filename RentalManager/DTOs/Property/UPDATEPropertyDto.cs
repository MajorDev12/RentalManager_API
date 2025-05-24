using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Property
{
    public class UPDATEPropertyDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string County { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Area { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string PhysicalAddress { get; set; } = string.Empty;

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        [Required]
        public int Floor { get; set; }

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        [MaxLength(50)]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; } = string.Empty;

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
