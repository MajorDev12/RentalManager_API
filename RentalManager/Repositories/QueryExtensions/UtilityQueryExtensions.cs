using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.UtilityBill;
using RentalManager.Models;
using System.Linq.Expressions;

namespace RentalManager.Repositories.QueryExtensions
{
    public static class UtilityQueryExtensions
    {
        // SEARCH
        public static IQueryable<UtilityBill> ApplyUtilitySearch(
            this IQueryable<UtilityBill> query,
            UtilityBillQueryFilter filter)
        {
            return query.ApplySearch(
                filter.SearchTerm,
                x => x.Utility.Item,
                x => x.Property.Name,
                x => x.BillingCycle.Item,
                x => x.Unit.Name
            );
        }

        // FILTERS
        public static IQueryable<UtilityBill> ApplyUtilityFilters(
           this IQueryable<UtilityBill> query,
           UtilityBillQueryFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.PropertyName))
            {
                var propertyName = filter.PropertyName.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Property.Name, $"{propertyName}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.UtilityName))
            {
                var utilityName = filter.UtilityName.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Utility.Item, $"{utilityName}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.BillingCycleName))
            {
                var BillingCycleName = filter.BillingCycleName.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.BillingCycle.Item, $"{BillingCycleName}%"));
            }

            return query;
        }


        // SORTING

        private static readonly Dictionary<string, Expression<Func<UtilityBill, object>>> SortMap
        = new()
        {
            ["id"] = x => x.Id,
            ["propertyName"] = x => x.Property.Name,
            ["name"] = x => x.Utility.Item,
            ["billingCycleName"] = x => x.BillingCycle.Item,
            ["isMetered"] = x => x.IsMetered,
        };

        public static IQueryable<UtilityBill> ApplyUtilitySorting(
        this IQueryable<UtilityBill> query,
        UtilityBillQueryFilter filter)
        {
            return query.ApplySorting(
                filter.SortBy,
                filter.IsDescending,
                SortMap
            );
        }
    }
}
