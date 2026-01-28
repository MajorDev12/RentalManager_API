using RentalManager.Data;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.QueryExtensions
{
    public static class PropertyQueryExtensions
    {
        public static IQueryable<Property> ApplyRoleFilter(
            this IQueryable<Property> query,
            ICurrentUser user,
            ApplicationDbContext context)
        {
            if (user.Role == "Landlord")
            {
                query = query.Where(u =>
                    context.PropertyAssignments.Any(pa =>
                        pa.UserId == user.UserId &&
                        pa.PropertyId == u.Id
                    )
                );
            }

            return query;
        }
    }
}
