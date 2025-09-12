namespace RentalManager.DTOs.UnitType
{
    public class UPDATEUnitTypeDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public int PropertyId { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}

