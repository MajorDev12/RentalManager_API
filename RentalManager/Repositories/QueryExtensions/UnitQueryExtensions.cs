using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.QueryExtensions
{
    public static class UnitQueryExtensions
    {
        public static IQueryable<Unit> ApplyRoleFilter(
            this IQueryable<Unit> query,
            ICurrentUser user,
            ApplicationDbContext context)
        {
            if (user.Role == "Landlord")
            {
                query = query.Where(u =>
                    context.PropertyAssignments.Any(pa =>
                        pa.UserId == user.UserId &&
                        pa.PropertyId == u.PropertyId
                    )
                );
            }

            return query;
        }


        public static IQueryable<Unit> WithDetails(
            this IQueryable<Unit> query)
        {
            return query
                .Include(ub => ub.Property)
                .Include(ub => ub.UnitType)
                .Include(ub => ub.Status);
        }
    }
}
