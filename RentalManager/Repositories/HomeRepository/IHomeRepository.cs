using RentalManager.DTOs.Home;

namespace RentalManager.Repositories.HomeRepository
{
    public interface IHomeRepository
    {
        Task<List<READHomeSummaryDto>> GetHomeSummaryAsync(HomeFilterDto filters);
    }
}
