namespace RentalManager.Models
{
    public class UtilityBill : AuditableEntity
    {
        public int Id { get; set; }

        public int UtilityId { get; set; }
        public SystemCodeItem Utility { get; set; } = null!;

        public decimal Amount { get; set; }

        public int BillingCycleId { get; set; }
        public SystemCodeItem BillingCycle { get; set; } = null!;

        public bool IsMetered { get; set; } = false;

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public int? UnitId { get; set; }
        public Unit? Unit{ get; set; }


        public ICollection<MeterReading> UtilityReadings = new List<MeterReading>();

    }

}
