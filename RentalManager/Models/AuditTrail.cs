namespace RentalManager.Models
{
    public class AuditTrail
    {
        public int Id { get; set; }

        public int AccountId { get; set; }
        public Account? Account { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public string EntityName { get; set; } = null!; // e.g. "Lease", "Transaction"
        public int EntityId { get; set; }

        public string Action { get; set; } = null!; // CREATE, UPDATE, DELETE

        public string? OldValues { get; set; } // JSON
        public string? NewValues { get; set; } // JSON

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
