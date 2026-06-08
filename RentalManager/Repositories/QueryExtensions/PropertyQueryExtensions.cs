using Microsoft.EntityFrameworkCore;
using RentalManager.DTOs.Property;
using RentalManager.Models;
using System.Linq.Expressions;

namespace RentalManager.Repositories.QueryExtensions
{
    public static class PropertyQueryExtensions
    {

        // SEARCH
        public static IQueryable<Property> ApplyPropertySearch(
            this IQueryable<Property> query,
            PropertyQueryFilter filter)
        {
            return query.ApplySearch(
                filter.SearchTerm,
                x => x.Name,
                x => x.Area,
                x => x.County,
                x => x.Country
            );
        }

        // FILTERS
        public static IQueryable<Property> ApplyPropertyFilters(
           this IQueryable<Property> query,
           PropertyQueryFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Country))
            {
                var country = filter.Country.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Country, $"{country}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.County))
            {
                var county = filter.County.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.County, $"{county}%"));
            }

            if (!string.IsNullOrWhiteSpace(filter.Area))
            {
                var area = filter.Area.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Area, $"{area}%"));
            }

            if (filter.PropertyTypeId.HasValue)
            {
                query = query.Where(x =>
                    x.PropertyTypeId == filter.PropertyTypeId.Value);
            }

            return query;
        }


        // SORTING
        private static readonly Dictionary<string, Expression<Func<Property, object>>> SortMap
        = new()
        {
            ["id"] = x => x.Id,
            ["name"] = x => x.Name,
            ["country"] = x => x.Country,
            ["county"] = x => x.County,
            ["area"] = x => x.Area,
        };

        public static IQueryable<Property> ApplyPropertySorting(
        this IQueryable<Property> query,
        PropertyQueryFilter filter)
        {
            return query.ApplySorting(
                filter.SortBy,
                filter.IsDescending,
                SortMap
            );
        }
         
    }
}
