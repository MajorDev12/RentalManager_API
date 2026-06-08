namespace RentalManager.DTOs.SystemCodeItem
{
    public class SystemCodeItemMetadata
    {
        public string Name { get; set; } = string.Empty;

        public string? IconKey { get; set; }

        public string? Color { get; set; }

        public int SortOrder { get; set; }
    }
}
