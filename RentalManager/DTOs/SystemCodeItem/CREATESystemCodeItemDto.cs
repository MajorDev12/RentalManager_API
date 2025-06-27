using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.SystemCodeItem
{
    public class CREATESystemCodeItemDto
    {
        [Required]
        [MaxLength(50)]
        public string Item { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Notes { get; set; }

        [Required]
        public int SystemCodeId { get; set; }
    }
}
