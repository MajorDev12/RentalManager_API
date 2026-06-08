using System.ComponentModel.DataAnnotations;

namespace RentalManager.DTOs.Unit
{
    public class CREATEUnitDto
    {
        public int PropertyId { get; set; }
        public int UnitTypeId { get; set; }
        public int BillingCycleId { get; set; }
        public int RentalTypeId { get; set; }
        public int Floor { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string? Notes { get; set; }
    }
}
