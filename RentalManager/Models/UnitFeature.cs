using RentalManager.Constants;

namespace RentalManager.Models
{
    public class UnitFeature : AuditableEntity
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Icon { get; set; }

        public FeatureDataType DataType { get; set; }

        public bool IsActive { get; set; } = true;


        // Navigation
        public Account Account { get; set; } = null!;

        public ICollection<UnitFeatureAssignment> UnitAssignments { get; set; }
            = new List<UnitFeatureAssignment>();
    }
}
