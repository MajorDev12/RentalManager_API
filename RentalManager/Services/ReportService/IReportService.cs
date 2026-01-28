using RentalManager.Models;
using RentalManager.DTOs.Report;

namespace RentalManager.Services.ReportService
{
    public interface IReportService
    {
        Task<ApiResponse<object>> GetReport(ReportFilterDto filter);
    }
}
