namespace RentalManager.DTOs.Report
{
    public class READReportDto
    {
        public string? PropertyName { get; set; }
        public string? UnitName { get; set; }
        public string? TenantName { get; set; }

        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetProfit { get; set; }

        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
