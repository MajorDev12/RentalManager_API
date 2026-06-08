using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Report;
using RentalManager.Models;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.ReportRepository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUser _currentuser;
        private readonly ISystemCodeItemRepository _systemcodeitemrepo;

        public ReportRepository(
            ApplicationDbContext context,
            ICurrentUser currentuser,
            ISystemCodeItemRepository systemcodeitemrepo)
        {
            _context = context;
            _currentuser = currentuser;
            _systemcodeitemrepo = systemcodeitemrepo;
        }


        public async Task<ReportResponseDto> GetReportAsync(ReportFilterDto filters)
        {
            var paymentType = await _systemcodeitemrepo.GetByCodeAndItemAsync("payment", "TRANSACTIONTYPE");
            var expenseType = await _systemcodeitemrepo.GetByCodeAndItemAsync("expense", "TRANSACTIONTYPE");

            var year = filters.Year ?? DateTime.UtcNow.Year;

            bool hasProperty = filters.PropertyId.HasValue && filters.PropertyId.Value > 0;
            bool hasUnit = filters.UnitId.HasValue && filters.UnitId.Value > 0;
            bool hasUser = filters.UserId.HasValue && filters.UserId.Value > 0;
            bool hasMonth = filters.Month.HasValue && filters.Month.Value > 0;

            var query = _context.Transactions
                .Include(t => t.Property)
                .Include(t => t.Unit)
                .Include(t => t.User)
                .Where(t => t.AccountId == _currentuser.AccountId)
                .Where(t => t.YearFor == year)
                .AsQueryable();

            if (hasProperty) query = query.Where(t => t.PropertyId == filters.PropertyId);
            if (hasUnit) query = query.Where(t => t.UnitId == filters.UnitId);
            if (hasUser) query = query.Where(t => t.UserId == filters.UserId);

            // ===========================
            // 1️⃣ NO FILTERS → SUMMARY
            // ===========================
            if (!hasProperty && !hasUnit && !hasUser && !hasMonth)
            {
                var income = await query
                    .Where(t => t.TransactionTypeId == paymentType.Id)
                    .SumAsync(t => (decimal?)t.Amount) ?? 0;

                var expense = await query
                    .Where(t => t.TransactionTypeId == expenseType.Id)
                    .SumAsync(t => (decimal?)t.Amount) ?? 0;

                return new ReportResponseDto
                {
                    IsSummary = true,
                    Summary = new READReportDto
                    {
                        TotalIncome = income,
                        TotalExpense = expense,
                        NetProfit = income - expense,
                        Year = year
                    }
                };
            }

            // ===========================
            // 2️⃣ MONTH FILTER → ONE ROW
            // ===========================
            if (hasMonth)
            {
                query = query.Where(t => t.MonthFor == filters.Month);

                var data = await query.ToListAsync();

                var income = data.Where(t => t.TransactionTypeId == paymentType.Id).Sum(t => t.Amount);
                var expense = data.Where(t => t.TransactionTypeId == expenseType.Id).Sum(t => t.Amount);

                return new ReportResponseDto
                {
                    IsSummary = true,
                    Summary = new READReportDto
                    {
                        PropertyName = data.FirstOrDefault()?.Property?.Name,
                        UnitName = data.FirstOrDefault()?.Unit?.Name,
                        TenantName = data.FirstOrDefault()?.User != null
                            ? $"{data.First().User.FirstName} {data.First().User.LastName}"
                            : null,
                        TotalIncome = income,
                        TotalExpense = expense,
                        NetProfit = income - expense,
                        Month = filters.Month,
                        Year = year
                    }
                };
            }

            // ===========================
            // 3️⃣ ENTITY → 12 MONTHS
            // ===========================
            var monthly = await query
                .GroupBy(t => t.MonthFor)
                .Select(g => new
                {
                    Month = g.Key,
                    Income = g.Where(x => x.TransactionTypeId == paymentType.Id).Sum(x => x.Amount),
                    Expense = g.Where(x => x.TransactionTypeId == expenseType.Id).Sum(x => x.Amount),
                    PropertyName = g.Select(x => x.Property!.Name).FirstOrDefault(),
                    UnitName = g.Select(x => x.Unit!.Name).FirstOrDefault(),
                    TenantName = g.Select(x => x.User != null
                        ? x.User.FirstName + " " + x.User.LastName
                        : null).FirstOrDefault()
                })
                .ToListAsync();

            var result = Enumerable.Range(1, 12)
                .Select(m =>
                {
                    var row = monthly.FirstOrDefault(x => x.Month == m);
                    return new READReportDto
                    {
                        PropertyName = row?.PropertyName,
                        UnitName = row?.UnitName,
                        TenantName = row?.TenantName,
                        TotalIncome = row?.Income ?? 0,
                        TotalExpense = row?.Expense ?? 0,
                        NetProfit = (row?.Income ?? 0) - (row?.Expense ?? 0),
                        Month = m,
                        Year = year
                    };
                })
                .ToList();

            return new ReportResponseDto
            {
                IsSummary = false,
                Monthly = result
            };
        }

    }
}
