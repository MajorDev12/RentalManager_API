namespace RentalManager.DTOs.AuditTrail
{
    public class CREATEAuditDto
    {
        public int? UserId { get; set; }
        public int EntityId { get; set; }

        public string EntityName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}
