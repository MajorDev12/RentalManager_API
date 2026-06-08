namespace RentalManager.DTOs.AuditTrail
{
    public class READAuditTrailDto
    {
        public int EntityId { get; set; }

        public string EntityName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
