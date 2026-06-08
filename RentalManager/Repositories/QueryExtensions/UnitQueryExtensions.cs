using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.DTOs.Property;
using RentalManager.DTOs.Unit;
using RentalManager.Models;
using System.Linq.Expressions;

namespace RentalManager.Repositories.QueryExtensions
{
    public static class UnitQueryExtensions
    {

        public static IQueryable<Unit> WithDetails(
            this IQueryable<Unit> query)
        {
            return query
                .Include(ub => ub.Property)
                .Include(ub => ub.UnitType)
                .Include(ub => ub.RentalType)
                .Include(ub => ub.BillingCycle)
                .Include(ub => ub.Status)
                .Include(ub => ub.Leases);
        }


        // SEARCH
        public static IQueryable<Unit> ApplyUnitSearch(
            this IQueryable<Unit> query,
            UnitQueryFilter filter)
        {
            return query.ApplySearch(
                filter.SearchTerm,
                x => x.Property.Name,
                x => x.UnitType.Item,
                //x => x.Amount,
                x => x.Status.Item
            );
        }

        // FILTERS
        public static IQueryable<Unit> ApplyUnitFilters(
           this IQueryable<Unit> query,
           UnitQueryFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.PropertyName))
            {
                var propertyName = filter.PropertyName.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Property.Name, $"{propertyName}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                var Name = filter.Name.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, $"{Name}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.UnitType))
            {
                var unitType = filter.UnitType.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.UnitType.Item, $"{unitType}%"));
            }

            //if (filter.Amount.HasValue)
            //{
            //    query = query.Where(x =>
            //        x.Amount == filter.Amount.Value);
            //}


            if (filter.UnitStatus.HasValue)
            {
                query = query.Where(x =>
                    x.Status.Id == filter.UnitStatus.Value);
            }

            return query;
        }


        // SORTING
        private static readonly Dictionary<string, Expression<Func<Unit, object>>> SortMap
        = new()
        {
            ["id"] = x => x.Id,
            ["propertyName"] = x => x.Property.Name,
            ["name"] = x => x.Name,
            ["unitType"] = x => x.UnitType.Item,
            ["amount"] = x => x.Amount,
            ["status"] = x => x.Status.Item,
        };

        public static IQueryable<Unit> ApplyUnitSorting(
        this IQueryable<Unit> query,
        UnitQueryFilter filter)
        {
            return query.ApplySorting(
                filter.SortBy,
                filter.IsDescending,
                SortMap
            );
        }
    }
}
