namespace RentalManager.DTOs.Report
{
    public class ReportResponseDto
    {
        public bool IsSummary { get; set; }
        public READReportDto? Summary { get; set; }
        public List<READReportDto>? Monthly { get; set; }
    }

}
