using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Unit
{
    public class CREATEUnitDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Amount { get; set; }

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        public int UnitTypeId { get; set; }

        [Required]
        public int PropertyId { get; set; }
    }
}
