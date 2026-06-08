using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Models;
using RentalManager.Services.AccountAccessService;

namespace RentalManager.Repositories.QueryExtensions
{
    public static class TenantQueryExtensions
    {
        public static IQueryable<Tenant> ApplyRoleFilter(
            this IQueryable<Tenant> query,
            ICurrentUser user,
            ApplicationDbContext context
            )
        {
            return user.Role switch
            {
                "Owner" or "Manager" => query,

                "Tenant" => query.Where(t => t.UserId == user.UserId),

                "Landlord" => query.Where(t =>
                    context.PropertyAssignments.Any(pa =>
                        pa.UserId == user.UserId &&
                        pa.PropertyId == t.Unit.PropertyId
                    )
                ),

                _ => query.Where(_ => false)
            };
        }


        public static IQueryable<Tenant> ByProperty(
            this IQueryable<Tenant> query,
            int propertyId)
        {
            return query.Where(t => t.User.PropertyId == propertyId);
        }


        public static IQueryable<Tenant> ByUnit(
            this IQueryable<Tenant> query,
            int unitId)
        {
            return query.Where(t => t.UnitId == unitId);
        }


        public static IQueryable<Tenant> IsActive(
            this IQueryable<Tenant> query)
        {
            return query.Where(t => t.TenantStatus.Item == "Active");
        }


        public static IQueryable<Tenant> WithDetails(
            this IQueryable<Tenant> query)
        {
            return query
                .Include(t => t.Unit)
                .Include(t => t.User)
                    .ThenInclude(u => u.Gender)
                .Include(t => t.User)
                    .ThenInclude(u => u.UserStatus)
                .Include(t => t.User)
                    .ThenInclude(u => u.Property)
                .Include(t => t.TenantStatus);
        }

    }
}
