using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Unit
{
    public class CREATEUnitDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Status { get; set; }

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        public int UnitTypeId { get; set; }

        [Required]
        public int PropertyId { get; set; }
    }
}
