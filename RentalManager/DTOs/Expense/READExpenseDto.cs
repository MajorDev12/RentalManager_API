namespace RentalManager.DTOs.Expense
{
    public class READExpenseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string? Notes { get; set; }
    }
}
