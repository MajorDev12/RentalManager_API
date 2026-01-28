namespace RentalManager.Models
{
    public class PropertyAssignment : AuditableEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int PropertyId { get; set; }
        public Property Property { get; set; } = null!;

        public string AssignmentType { get; set; } = "Landlord";
    }
}
