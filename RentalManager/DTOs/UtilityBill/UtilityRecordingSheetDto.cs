namespace RentalManager.DTOs.UtilityBill
{
    public class UtilityRecordingSheetDto
    {
        public int UtilityId { get; set; }
        public string UtilityName { get; set; } = string.Empty;
        public bool IsPropertyWide { get; set; }

        public List<UtilityRecordingUnitDto> Units { get; set; } = new();
    }
}
