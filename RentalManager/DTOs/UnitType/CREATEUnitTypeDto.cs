using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.UnitType
{
    public class CREATEUnitTypeDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        public int PropertyId { get; set; }
    }
}

