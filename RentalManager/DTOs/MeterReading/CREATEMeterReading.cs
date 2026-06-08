namespace RentalManager.DTOs.MeterReading
{
    public class CREATEMeterReading
    {
        public int UnitId { get; set; }
        public int UtilityBillId { get; set; }

        public decimal PreviousReading { get; set; }

        public decimal CurrentReading { get; set; }

        public DateTime ReadingDate { get; set; }
    }
}
