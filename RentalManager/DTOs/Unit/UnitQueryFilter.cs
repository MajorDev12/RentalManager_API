using RentalManager.DTOs.Commons;

namespace RentalManager.DTOs.Unit
{
    public class UnitQueryFilter : QueryFilter
    {
        public string? Name { get; set; }
        public string? PropertyName { get; set; }
        public string? UnitType { get; set; }
        public double? Amount { get; set; }
        public int? UnitStatus { get; set; }
    }
}
