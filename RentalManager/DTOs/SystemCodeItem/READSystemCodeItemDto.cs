using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.SystemCodeItem
{
    public class READSystemCodeItemDto
    {
        public int Id { get; set; }
        public string Item { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
