namespace RentalManager.DTOs.Unit
{
    public class PATCHUnitDto
    {
        public string? Name { get; set; } 

        public decimal? Amount { get; set; }

        public string? Notes { get; set; }

        public int? UnitTypeId { get; set; }

        public int? RentalTypeId { get; set; }

        public int? BillingCycleId { get; set; }

        public int? Floor { get; set; }

        public int? PropertyId { get; set; }

        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
