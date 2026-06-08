using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Notification.Models;

namespace RentalManager.Notification.Repositories
{
    public class NotificationPreferenceRepository
        : INotificationPreferenceRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationPreferenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationPreference?> GetPreferencesForEventAsync(
            string eventType,
            int accountId,
            int userId)
        {
            return await _context.Set<NotificationPreference>()
                .IgnoreQueryFilters()
                .Where(p =>
                    p.AccountId == accountId &&
                    p.EventType == eventType &&
                    p.IsEnabled &&
                    (
                        // User-specific preference
                        p.UserId == userId 
                    )
                )
                .OrderByDescending(p => p.UserId.HasValue)
                .FirstOrDefaultAsync();
        }


        public async Task<bool> ExistsAsync(
            int accountId,
            int userId,
            string eventType)
        {
            return await _context.Set<NotificationPreference>()
                .AnyAsync(p =>
                    p.AccountId == accountId &&
                    p.UserId == userId &&
                    p.EventType == eventType);
        }



        public async Task AddAsync(NotificationPreference pref)
        {
            _context.Add(pref);
            await _context.SaveChangesAsync();
        }

        
    }
}
