using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Notification.Defaults;
using RentalManager.Notification.Events;

namespace RentalManager.Notification.Audience
{
    public class TenantAddedAudienceResolver
        : INotificationAudienceResolver
    {
        private readonly ApplicationDbContext _context;

        public TenantAddedAudienceResolver(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<NotificationRecipient>> ResolveAsync(
            INotificationEvent @event)
        {
            if (@event is not TenantAddedEvent e)
                return Array.Empty<NotificationRecipient>();

            // 1️⃣ Owner(s) of the account
            var owners = await _context.Users
                .IgnoreQueryFilters()
                .Where(u =>
                    u.AccountId == e.AccountId &&
                    u.Account.Name == NotificationConstants.Role.Owner)
                .Select(u => new NotificationRecipient
                {
                    UserId = u.Id,
                    Role = NotificationConstants.Role.Owner
                })
                .ToListAsync();


            var managers = await _context.Users
                .IgnoreQueryFilters()
                .Where(u =>
                    u.AccountId == e.AccountId &&
                    u.Account.Name == NotificationConstants.Role.Manager)
                .Select(u => new NotificationRecipient
                {
                    UserId = u.Id,
                    Role = NotificationConstants.Role.Manager
                })
                .ToListAsync();

            // 2️⃣ Landlord(s) assigned to property
            var landlords = await _context.PropertyAssignments
                .Where(p => p.PropertyId == e.PropertyId)
                .Select(p => new NotificationRecipient
                {
                    UserId = p.UserId,
                    Role = NotificationConstants.Role.Landlord
                })
                .ToListAsync();


            var tenant = await _context.Tenants
                .Where(p => p.UserId == e.TenantUserId)
                .Select(p => new NotificationRecipient
                {
                    UserId = p.UserId,
                    Role = NotificationConstants.Role.Tenant
                })
                .FirstOrDefaultAsync();

            var recipients = owners
                .Concat(managers)
                .Concat(landlords)
                .ToList();

            if (tenant != null)
                recipients.Add(tenant);

            return recipients
                .DistinctBy(r => r.UserId)
                .ToList();
        }
    }
}
