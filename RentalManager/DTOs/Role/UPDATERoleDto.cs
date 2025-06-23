using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Role
{
    public class UPDATERoleDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

        [Required]
        public int PropertyId { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
