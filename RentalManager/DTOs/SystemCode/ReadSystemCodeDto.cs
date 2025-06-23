using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.SystemCode
{
    public class READSystemCodeDto
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string? Notes { get; set; }

    }
}
