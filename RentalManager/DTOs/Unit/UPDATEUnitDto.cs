namespace RentalManager.DTOs.Unit
{
    public class UPDATEUnitDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Status { get; set; }

        public string? Notes { get; set; }

        public int UnitTypeId { get; set; }

        public int PropertyId { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
