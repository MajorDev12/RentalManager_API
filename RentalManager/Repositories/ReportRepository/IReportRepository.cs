using RentalManager.DTOs.Report;
using RentalManager.Models;

namespace RentalManager.Repositories.ReportRepository
{
    public interface IReportRepository
    {
        Task<List<READReportDto>> GetReportAsync(ReportFilterDto filters);
    }
}
