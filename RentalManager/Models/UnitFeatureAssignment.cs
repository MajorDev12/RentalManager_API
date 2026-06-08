namespace RentalManager.Models
{
    public class UnitFeatureAssignment : AuditableEntity
    {
        public int Id { get; set; }

        public int UnitId { get; set; }

        public int UnitFeatureId { get; set; }

        public string? Value { get; set; }


        // Navigation
        public Unit Unit { get; set; } = null!;

        public UnitFeature UnitFeature { get; set; } = null!;
    }
}
