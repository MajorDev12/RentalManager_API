
namespace RentalManager.Models
{
    public class Unit : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string? Notes { get; set; }

        public int Floor { get; set; } = 0;

        public int BillingCycleId { get; set; }
        public SystemCodeItem BillingCycle { get; set; } = null!;

        public int RentalTypeId { get; set; }
        public SystemCodeItem RentalType { get; set; } = null!;


        public int UnitTypeId { get; set; }
        public SystemCodeItem UnitType { get; set; } = null!;

        public int StatusId { get; set; }
        public SystemCodeItem Status { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public int AccountId {  get; set; }
        public Account Account { get; set; } = null!;

        public ICollection<Lease> Leases { get; set; } = new List<Lease>();
        public ICollection<UtilityBill> UtilityBills { get; set; } = new List<UtilityBill>();
        public ICollection<UnitFeatureAssignment> Features { get; set; }
            = new List<UnitFeatureAssignment>();
    }

}
