

namespace RentalManager.DTOs.UnitType
{
    public class READUnitTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Notes { get; set; }

        public decimal Amount { get; set; }

        public int PropertyId { get; set; }

        public string PropertyName { get; set; } = string.Empty;
    }
}
