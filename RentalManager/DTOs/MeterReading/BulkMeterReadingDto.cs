namespace RentalManager.DTOs.MeterReading
{
    public class BulkMeterReadingDto
    {
        public List<BulkMeterReadingItemDto> Readings { get; set; } = new();
    }
}
