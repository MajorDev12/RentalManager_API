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




        public async Task<ApiResponse<object>> GetReport(ReportFilterDto filter)
        {
            try 
            {
                var data = await _repo.GetReportAsync(filter);

                return new ApiResponse<object>(data, "");
            }catch (Exception ex)
            {
                return new ApiResponse<object>(null, $"Error Occured: {ex.InnerException?.Message ?? ex.Message}");
            }

        }



    }
}
