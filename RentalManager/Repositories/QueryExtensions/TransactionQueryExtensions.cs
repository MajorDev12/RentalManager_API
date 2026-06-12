using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Transaction;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Extensions.Query
{
    public static class TransactionQueryExtensions
    {

        public static IQueryable<Transaction> ApplyRoleFilter(
            this IQueryable<Transaction> query,
            ICurrentUser user,
            ApplicationDbContext context)
        {
            if (user.Role == "Tenant")
            {
                query = query.Where(t => t.UserId == user.UserId);
            }

            if (user.Role == "Landlord")
            {
                query = query.Where(t =>
                    context.PropertyAssignments.Any(pa =>
                        pa.UserId == user.UserId &&
                        pa.PropertyId == t.PropertyId
                    )
                );
            }

            // Owner / Manager → unrestricted
            return query;
        }


        public static IQueryable<Transaction> WithDetails(
            this IQueryable<Transaction> query)
        {
            return query
                .Include(t => t.User)
                .Include(t => t.Unit)
                .Include(t => t.Property)
                .Include(t => t.TransactionCategory)
                .Include(t => t.TransactionType);
                //.Include(t => t.UtilityBill)
                //.Include(t => t.ExpenseCategory);
        }



        public static IQueryable<Transaction> ByMonth(
            this IQueryable<Transaction> query,
            int month,
            int year)
        {
            return query.Where(t => t.MonthFor == month && t.YearFor == year);
        }



        public static IQueryable<Transaction> ByProperty(
            this IQueryable<Transaction> query,
            int propertyId)
        {
            return query.Where(t => t.PropertyId == propertyId);
        }



        public static IQueryable<Transaction> ByCategory(
            this IQueryable<Transaction> query,
            int? category)
        {
            return query.Where(t => t.TransactionCategory.Id == category);
        }



        public static IQueryable<Transaction> ByUtilities(
            this IQueryable<Transaction> query,
            List<UtilityBill>? utilities)
        {
            if (utilities == null || utilities.Count == 0)
                return query;

            var utilityIds = utilities
                .Where(u => u != null)
                .Select(u => u.Id)
                .ToList();

            return query.Where(t =>
                t.UnitId.HasValue &&
                utilityIds.Contains(t.UnitId.Value));
        }


        public static IQueryable<Transaction> WithUserOnly(
            this IQueryable<Transaction> query)
        {
            return query.Where(t => t.UserId != null);
        }




        public static IQueryable<TenantBalanceDto> ToTenantBalance(
    this IQueryable<IGrouping<TenantBalanceGroupKey, Transaction>> groups)
        {
            return groups.Select(g => new TenantBalanceDto
            {
                UserId = g.Key.UserId,

                FullName = g.Select(x =>
                        x.User.FirstName + " " + x.User.LastName)
                    .FirstOrDefault() ?? "",

                UnitName = g.Select(x => x.Unit.Name)
                    .FirstOrDefault() ?? "",

                PropertyName = g.Select(x => x.Unit.Property.Name)
                    .FirstOrDefault() ?? "",

                //CategoryName = g.Key.GroupKey,
                CategoryId = g.Key.CategoryId,

                Month = g.Key.MonthFor,
                Year = g.Key.YearFor,

                TotalCharges = g
                    .Where(t => t.TransactionType.Item == "Charge")
                    .Sum(t => t.Amount),

                TotalPayments = g
                    .Where(t => t.TransactionType.Item == "Payment")
                    .Sum(t => t.Amount),

                Balance =
                    g.Where(t => t.TransactionType.Item == "Charge").Sum(t => t.Amount)
                  - g.Where(t => t.TransactionType.Item == "Payment").Sum(t => t.Amount)
            });
        }





        public static IQueryable<TenantBalanceDto> WithPositiveBalance(
            this IQueryable<TenantBalanceDto> query)
        {
            return query.Where(b => b.Balance > 0);
        }



        public static IQueryable<TenantBalanceDto> OrderByPeriod(
            this IQueryable<TenantBalanceDto> query)
        {
            return query
                .OrderBy(b => b.Year)
                .ThenBy(b => b.Month)
                .ThenBy(b => b.CategoryName);
        }








    }
}
