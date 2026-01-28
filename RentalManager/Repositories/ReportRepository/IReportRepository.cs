using RentalManager.DTOs.Report;
using RentalManager.Models;

namespace RentalManager.Repositories.ReportRepository
{
    public interface IReportRepository
    {
        Task<ReportResponseDto> GetReportAsync(ReportFilterDto filters);
    }
}
