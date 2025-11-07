using RentalManager.Models;
using RentalManager.DTOs.Report;

namespace RentalManager.Services.ReportService
{
    public interface IReportService
    {
        Task<ApiResponse<List<READReportDto>>> GetReport(ReportFilterDto filter);
    }
}
