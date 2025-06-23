using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.SystemCode
{
    public class CREATESystemCodeDto
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;


        [MaxLength(100)]
        public string? Notes { get; set; }
    }
}
