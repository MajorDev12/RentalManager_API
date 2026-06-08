namespace RentalManager.DTOs.UtilityBill
{
    public class PATCHUtilityDto
    {
        public int? UtilityId { get; set; }
        public int? BillingCycleId { get; set; }

        public decimal? Amount { get; set; }

        public bool? IsMetered { get; set; }

        public int? PropertyId { get; set; }
        public int? UnitId { get; set; }
    }
}
