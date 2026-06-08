namespace RentalManager.Models
{
    public class TransactionAllocation
    {
        public int Id { get; set; }

        public int PaymentTransactionId { get; set; }
        public Transaction PaymentTransaction { get; set; } = null!;

        public int ChargeTransactionId { get; set; }
        public Transaction ChargeTransaction { get; set; } = null!;

        public decimal Amount { get; set; }
    }
}
