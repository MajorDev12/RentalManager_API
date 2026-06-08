using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;
using System.Linq.Expressions;
using System.Reflection;

namespace RentalManager.Repositories.QueryExtensions
{
    public static class QueryableExtensions
    {

        public static IQueryable<T> ApplyRoleFilter<T>(
        this IQueryable<T> query,
        ICurrentUser user,
        ApplicationDbContext context,
        Expression<Func<T, int>> propertyIdSelector)
        {
            if (user.Role == Constants.RoleNames.Manager ||
                user.Role == Constants.RoleNames.Landlord)
            {
                var assignedPropertyIds = context.PropertyAssignments
                    .Where(pa => pa.UserId == user.UserId)
                    .Select(pa => pa.PropertyId);

                query = query.Where(BuildContainsExpression(
                    propertyIdSelector,
                    assignedPropertyIds));
            }

            return query;
        }

        private static Expression<Func<T, bool>> BuildContainsExpression<T>(
            Expression<Func<T, int>> selector,
            IQueryable<int> values)
        {
            var parameter = selector.Parameters[0];

            var body = Expression.Call(
                typeof(Queryable),
                nameof(Queryable.Contains),
                new[] { typeof(int) },
                values.Expression,
                selector.Body);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }


        // =========================
        // SEARCH
        // =========================
        public static IQueryable<T> ApplySearch<T>(
            this IQueryable<T> query,
            string? searchTerm,
            params Expression<Func<T, string>>[] properties)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) || properties.Length == 0)
                return query;

            searchTerm = searchTerm.Trim();

            var parameter = Expression.Parameter(typeof(T), "x");

            Expression? combined = null;

            foreach (var prop in properties)
            {
                var body = new ReplaceParameterVisitor(
                    prop.Parameters[0],
                    parameter).Visit(prop.Body);

                var likeMethod = typeof(DbFunctionsExtensions)
                    .GetMethod(nameof(DbFunctionsExtensions.Like),
                    new[]
                    {
                    typeof(DbFunctions),
                    typeof(string),
                    typeof(string)
                    });

                var efFunctions = Expression.Property(
                    null,
                    typeof(EF),
                    nameof(EF.Functions));

                var pattern = Expression.Constant($"%{searchTerm}%");

                var coalesce = Expression.Coalesce(
                    body!,
                    Expression.Constant(""));

                var likeCall = Expression.Call(
                    likeMethod!,
                    efFunctions,
                    coalesce,
                    pattern);

                combined = combined == null
                    ? likeCall
                    : Expression.OrElse(combined, likeCall);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(
                combined!,
                parameter);

            return query.Where(lambda);
        }

        // =========================
        // PAGINATION
        // =========================
        public static IQueryable<T> ApplyPagination<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            pageNumber = pageNumber < 1 ? 1 : pageNumber;

            pageSize = pageSize switch
            {
                < 1 => 10,
                > 100 => 100,
                _ => pageSize
            };

            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        // =========================
        // SORTING
        // =========================
        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string? sortBy,
            bool isDescending,
            Dictionary<string, Expression<Func<T, object>>> columnMap)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            sortBy = sortBy.Trim().ToLower();

            if (!columnMap.TryGetValue(sortBy, out var expression))
                return query;

            return isDescending
                ? query.OrderByDescending(expression)
                : query.OrderBy(expression);
        }


        // 🔧 PARAMETER REPLACER (REQUIRED FOR EF TRANSLATION)
        internal class ReplaceParameterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly ParameterExpression _newParam;

            public ReplaceParameterVisitor(ParameterExpression oldParam, ParameterExpression newParam)
            {
                _oldParam = oldParam;
                _newParam = newParam;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParam ? _newParam : base.VisitParameter(node);
            }
        }


    }
}