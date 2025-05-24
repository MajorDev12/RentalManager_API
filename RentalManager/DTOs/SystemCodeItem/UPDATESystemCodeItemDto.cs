using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.SystemCodeItem
{
    public class UPDATESystemCodeItemDto
    {
       
        [Required]
        [MaxLength(50)]
        public string Item { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Notes { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
