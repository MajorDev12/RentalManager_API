namespace RentalManager.Models
{
    public class Expense : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string? Notes { get; set; }

        public int PropertyId { get; set; }

        public Property Property { get; set; } = null!;

    }
}
