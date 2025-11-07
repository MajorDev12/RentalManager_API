namespace RentalManager.DTOs.Report
{
    public class ReportFilterDto
    {
        public string ReportType { get; set; } = "Income"; // or "Expense"
        public int? PropertyId { get; set; }
        public int? UnitId { get; set; }
        public int? UserId { get; set; }  
        public int? Month { get; set; }
        public int? Year { get; set; }
    }

}
