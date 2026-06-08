namespace RentalManager.DTOs.Unit
{
    public class READUnitDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int StatusId { get; set; }

        public string Status { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string? Notes { get; set; }


        public int BillingCycleId { get; set; }
        public string BillingCycle { get; set; } = string.Empty;

        public int RentalTypeId { get; set; }
        public string RentalType { get; set; } = string.Empty;

        public int Floor { get; set; }

        public int UnitTypeId { get; set; }
        public string UnitType { get; set; } = string.Empty;

        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = string.Empty;
    }
}
