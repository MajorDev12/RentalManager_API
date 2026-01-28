using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Home;

namespace RentalManager.Repositories.HomeRepository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<READHomeSummaryDto>> GetHomeSummaryAsync(HomeFilterDto filters)
        {
            var properties = _context.Properties
                .AsQueryable();

            var tenants = _context.Tenants
                .AsQueryable();

            var houses = _context.Units
                .AsQueryable();

            var vacants = houses.Where(t => t.Status.Item.ToLower() == "vacant");


            // Apply filters
            if (filters.PropertyId.HasValue)
                properties = properties.Where(t => t.Id == filters.PropertyId);
                tenants = tenants.Where(t => t.User!.PropertyId == filters.PropertyId);
                houses = houses.Where(t => t.PropertyId == filters.PropertyId);
                vacants = vacants.Where(t => t.PropertyId == filters.PropertyId);



            // ✅ If month is NOT provided — get report per month for the whole year
            var totalSummary = await properties
                .Select(g => new READHomeSummaryDto
                {
                    totalProperties = properties.Count(),
                })
                .ToListAsync();

            totalSummary = await houses
                .Select(g => new READHomeSummaryDto
                {
                    totalHouses = houses.Count(),
                })
                .ToListAsync();

            totalSummary = await tenants
                .Select(g => new READHomeSummaryDto
                {
                    totalActiveTenants = tenants.Count(),
                })
                .ToListAsync();

            totalSummary = await vacants
                .Select(g => new READHomeSummaryDto
                {
                    totalVacants = vacants.Count(),
                    totalLandlords = properties.Count(),
                })
                .ToListAsync();

            return totalSummary;
        }

    }
}
