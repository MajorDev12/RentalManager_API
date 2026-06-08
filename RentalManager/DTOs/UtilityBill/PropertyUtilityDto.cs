namespace RentalManager.DTOs.UtilityBill
{
    public class PropertyUtilityDto
    {
        public int UtilityId { get; set; }
        public string UtilityName { get; set; } = string.Empty;
        public bool IsPropertyWide { get; set; }
        public bool HasUnitSpecificConfigurations { get; set; }
    }
}
