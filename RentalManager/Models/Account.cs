namespace RentalManager.Models
{
    public class Account : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public bool IsTrial { get; set; } = true;

        public DateTime TrialEndsAt { get; set; }


        public DateTime? SubscriptionEndsAt { get; set; }

        public ICollection<Property> Properties { get; set; } = new List<Property>();

    }
}
