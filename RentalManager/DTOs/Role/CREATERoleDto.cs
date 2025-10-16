using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Role
{
    public class CREATERoleDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;
    }
}
