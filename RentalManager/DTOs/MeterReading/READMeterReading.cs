namespace RentalManager.DTOs.MeterReading
{
    public class READMeterReading
    {
        public int UtilityBillId { get; set; }

        public decimal PreviousReading { get; set; }

        public decimal CurrentReading { get; set; }
    }
}
