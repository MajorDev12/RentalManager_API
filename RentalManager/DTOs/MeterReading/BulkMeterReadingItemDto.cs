namespace RentalManager.DTOs.MeterReading
{
    public class BulkMeterReadingItemDto
    {
        public int UtilityBillId { get; set; }
        public int UnitId { get; set; }
        public decimal CurrentReading { get; set; }
    }
}
