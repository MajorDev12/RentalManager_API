namespace RentalManager.DTOs.UnitType
{
    public class UPDATEUnitTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public decimal Amount { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}

