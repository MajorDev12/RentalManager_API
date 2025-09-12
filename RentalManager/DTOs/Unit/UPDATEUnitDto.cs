namespace RentalManager.DTOs.Unit
{
    public class UPDATEUnitDto
    {
        public string Name { get; set; } = string.Empty;

        public int StatusId { get; set; }

        public int Amount { get; set; }

        public string? Notes { get; set; }

        public int UnitTypeId { get; set; }

        public int PropertyId { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
