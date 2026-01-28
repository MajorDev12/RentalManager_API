using RentalManager.DTOs.Home;
using RentalManager.DTOs.Report;
using RentalManager.Models;
using RentalManager.Repositories.HomeRepository;
using RentalManager.Repositories.ReportRepository;
using RentalManager.Repositories.RoleRepository;

namespace RentalManager.Services.HomeService
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _repo;


        public HomeService(IHomeRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<List<READHomeSummaryDto>>> GetSummary(HomeFilterDto filter)
        {
            try
            {
                // get total properties
                // get total Houses
                // get total tenants
                // get total vacants
                // get total landlords
                var totalSummary = await _repo.GetHomeSummaryAsync(filter);



                return new ApiResponse<List<READHomeSummaryDto>>(totalSummary, "Successfuly retrieved data");
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<READHomeSummaryDto>>(null, $"Error Occured: {ex.InnerException?.Message ?? ex.Message}");
            }

        }


    }
}
