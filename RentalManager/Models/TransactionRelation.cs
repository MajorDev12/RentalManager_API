namespace RentalManager.Models
{
    public class TransactionRelation
    {
        public int Id { get; set; }

        public int FromTransactionId { get; set; }
        public Transaction FromTransaction { get; set; } = null!;

        public int ToTransactionId { get; set; }
        public Transaction ToTransaction { get; set; } = null!;

        public int RelationTypeId { get; set; }
        public SystemCodeItem RelationType { get; set; } = null!;

        public string? Notes { get; set; }
    }
}
