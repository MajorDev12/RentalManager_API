namespace RentalManager.Models
{
    public class MaintenanceRequest : AuditableEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public int PropertyId { get; set; }

        public int? UnitId { get; set; }

        public int? TenantId { get; set; }

        public int? AssignedToUserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int StatusId { get; set; }
        public SystemCodeItem Status { get; set; } = null!;

        public DateTime RequestedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public virtual Account Account { get; set; } = default!;
        public virtual Property Property { get; set; } = default!;
        public virtual Unit? Unit { get; set; }
        public virtual Tenant? Tenant { get; set; }
        public virtual User? AssignedToUser { get; set; }
    }
}