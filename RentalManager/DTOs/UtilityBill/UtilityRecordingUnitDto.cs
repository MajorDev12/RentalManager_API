namespace RentalManager.DTOs.UtilityBill
{
    public class UtilityRecordingUnitDto
    {
        public int UtilityBillId { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; } = string.Empty;

        public decimal PreviousReading { get; set; }
        public decimal CurrentReading { get; set; }

        public decimal Rate { get; set; }

        public bool IsOverride { get; set; }
    }
}
