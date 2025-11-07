namespace RentalManager.DTOs.Expense
{
    public class UPDATEExpenseDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string? Notes { get; set; }

        public int PropertyId { get; set; }
    }
}
