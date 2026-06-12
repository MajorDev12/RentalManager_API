namespace RentalManager.DTOs.SystemCodeItem
{
    public class SystemCodeItemMetadata
    {
        public string DisplayName { get; set; } = string.Empty;

        public string? IconKey { get; set; }

        public string? Color { get; set; }

        public string? GroupKey { get; set; }

        public int SortOrder { get; set; }
    }
}
