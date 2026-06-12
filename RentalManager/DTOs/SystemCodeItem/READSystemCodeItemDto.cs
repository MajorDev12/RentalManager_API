using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.SystemCodeItem
{
    public class READSystemCodeItemDto
    {
        public int Id { get; set; }
        public string Item { get; set; } = string.Empty;
        public string? DisplayName { get; set; } = string.Empty;
        public string? Color { get; set; } = string.Empty;
        public string? IconKey { get; set; } = string.Empty;
        public string? GroupKey { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public int SystemCodeId { get; set; }

        public string SystemCodeName { get; set; } = string.Empty;
    }
}
