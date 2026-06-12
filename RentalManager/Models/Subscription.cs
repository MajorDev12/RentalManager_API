namespace RentalManager.Models
{
    public class Subscription : AuditableEntity
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string PlanName { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsTrial { get; set; }

        public bool IsActive { get; set; }

        public virtual Account Account { get; set; } = default!;
    }
}