using RentalManager.DTOs.Report;
using RentalManager.Models;
using RentalManager.Repositories.ReportRepository;

namespace RentalManager.Services.ReportService
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;


        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }




        public async Task<ApiResponse<List<READReportDto>>> GetReport(ReportFilterDto filter)
        {
            try 
            {
                var data = await _repo.GetReportAsync(filter);

                return new ApiResponse<List<READReportDto>>(data, "");
            }catch (Exception ex)
            {
                return new ApiResponse<List<READReportDto>>(null, $"Error Occured: {ex.InnerException?.Message ?? ex.Message}");
            }

        }



    }
}
