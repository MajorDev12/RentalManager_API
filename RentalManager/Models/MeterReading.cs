namespace RentalManager.Models
{
    public class MeterReading : AuditableEntity
    {
        public int Id { get; set; }

        public int UtilityBillId { get; set; }
        public UtilityBill UtilityBill { get; set; } = null!;

        public decimal PreviousReading { get; set; }

        public decimal CurrentReading { get; set; }

        public decimal UnitsConsumed { get; set; }

        public decimal Rate { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime ReadingDate { get; set; }

        public string? Notes { get; set; }

    }
}
