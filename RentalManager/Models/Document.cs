namespace RentalManager.Models
{
    public class Document : AuditableEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public int? PropertyId { get; set; }

        public int? UnitId { get; set; }

        public int? TenantId { get; set; }

        public int? LeaseId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string FilePath { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public virtual Account Account { get; set; } = default!;
        public virtual Property? Property { get; set; }
        public virtual Unit? Unit { get; set; }
        public virtual Tenant? Tenant { get; set; }
        public virtual Lease? Lease { get; set; }
    }
}