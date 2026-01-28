using RentalManager.Data;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.QueryExtensions
{
    public static class UtilityQueryExtensions
    {
        public static IQueryable<UtilityBill> ApplyRoleFilter(
            this IQueryable<UtilityBill> query,
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
    }
}
