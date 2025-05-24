

using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.SystemCode
{
    public class UpdateSystemCodeDto
    {
        [Required]
        [MaxLength(50)]
        public string Code { get; set; } = string.Empty;


        [MaxLength(100)]
        public string? Notes { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
