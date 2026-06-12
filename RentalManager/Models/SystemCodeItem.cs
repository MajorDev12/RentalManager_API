
namespace RentalManager.Models
{
    public class SystemCodeItem : AuditableEntity
    {
        public int Id { get; set; }

        public string Item { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string? IconKey { get; set; }

        public string? GroupKey { get; set; }

        public string? Color { get; set; }

        public int SortOrder { get; set; }

        public string? Notes { get; set; }


        public int SystemCodeId { get; set; }
        public SystemCode SystemCode { get; set; } = null!;

    }
}
