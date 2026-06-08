using RentalManager.DTOs.Commons;

namespace RentalManager.DTOs.Property
{
    public class PropertyQueryFilter : QueryFilter
    {
        public string? Country { get; set; }
        public string? County { get; set; }
        public string? Area { get; set; }

        public int? PropertyTypeId { get; set; }

        public bool? IsActive { get; set; }
    }
}
