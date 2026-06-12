namespace RentalManager.Models
{
    public class Booking : AuditableEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }

        public int PropertyId { get; set; }

        public int UnitId { get; set; }

        // Guest (can be existing user or external guest)
        public int? UserId { get; set; }

        public string GuestName { get; set; } = string.Empty;

        public string MobileNumber { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int NumberOfGuests { get; set; }

        public decimal NightlyRate { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal Balance => TotalAmount - AmountPaid;

        public int BookingStatusId { get; set; }

        public int? BookingSourceId { get; set; }

        public string? Notes { get; set; }

        public virtual Account Account { get; set; } = default!;
        public virtual Property Property { get; set; } = default!;
        public virtual Unit Unit { get; set; } = default!;
        public virtual User? User { get; set; }

        public virtual SystemCodeItem BookingStatus { get; set; } = default!;
        public virtual SystemCodeItem? BookingSource { get; set; }
    }
}