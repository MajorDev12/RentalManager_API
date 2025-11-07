using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Report;
using RentalManager.Models;

namespace RentalManager.Repositories.ReportRepository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<READReportDto>> GetReportAsync(ReportFilterDto filters)
        {
            var query = _context.Transactions
                .Include(t => t.Unit)
                .Include(t => t.Property)
                .Include(t => t.Expenses)
                .AsQueryable();

            // Apply filters
            if (filters.PropertyId.HasValue)
                query = query.Where(t => t.PropertyId == filters.PropertyId);

            if (filters.UnitId.HasValue)
                query = query.Where(t => t.UnitId == filters.UnitId);

            if (filters.UserId.HasValue)
                query = query.Where(t => t.UserId == filters.UserId);

            if (filters.Year.HasValue)
                query = query.Where(t => t.YearFor == filters.Year);


            var totalIncome = await query
                    .Where(t => t.TransactionType.Item.ToLower() == "payment")
                    .SumAsync(t => (decimal?)t.Amount) ?? 0;

            var totalExpense = await query
                .Where(t => t.Expenses.Amount > 0)
                .SumAsync(t => (decimal?)t.Amount) ?? 0;


            // ✅ If month is provided — single month report
            if (filters.Month.HasValue)
            {
                var monthQuery = query.Where(t => t.MonthFor == filters.Month);

                var dto = new READReportDto
                {
                    TotalIncome = totalIncome,
                    TotalExpense = totalExpense,
                    NetProfit = totalIncome - totalExpense,
                    Month = filters.Month,
                    Year = filters.Year
                };
                var reportList = new List<READReportDto>();
                reportList.Add(dto);
                return reportList;
            }

            // ✅ If month is NOT provided — get report per month for the whole year
            var monthlyReports = await query
                .GroupBy(t => t.MonthFor)
                .Select(g => new READReportDto
                {
                    TotalIncome = totalIncome,
                    TotalExpense = totalExpense,
                    NetProfit = totalIncome - totalExpense,
                    Month = g.Key,
                    Year = filters.Year
                })
                .OrderBy(r => r.Month)
                .ToListAsync();

            return monthlyReports;
        }

    }
}
