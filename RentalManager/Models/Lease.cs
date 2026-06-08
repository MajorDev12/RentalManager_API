namespace RentalManager.Models
{
    public class Lease
    {
        public int Id { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; } = null!;

        public int TenantId { get; set; }
        public Tenant Tenant { get; set; } = null!;

        public decimal RentAmount { get; set; }

        public int BillingCycleId { get; set; }
        public SystemCodeItem BillingCycle { get; set; } = null!;

        public DateTime NextBillingDate { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }

        public bool RequiresDeposit { get; set; }
        public decimal? DepositAmount { get; set; }

        public int LeaseStatusId { get; set; }
        public SystemCodeItem LeaseStatus { get; set; } = null!;
    }
}
