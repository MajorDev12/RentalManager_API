using RentalManager.DTOs.Home;
using RentalManager.Models;

namespace RentalManager.Services.HomeService
{
    public interface IHomeService
    {
        Task<ApiResponse<List<READHomeSummaryDto>>> GetSummary(HomeFilterDto filter);
    }
}
