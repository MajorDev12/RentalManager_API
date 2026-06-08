using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Notification.Models;

namespace RentalManager.Notification.Repositories
{
    public class NotificationRepository
        : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(InAppNotification notification)
        {
            _context.Set<InAppNotification>().Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<InAppNotification>> GetAllAsync(int userId)
        {
            return await _context.Set<InAppNotification>()
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<InAppNotification>> GetUnreadAsync(int userId)
        {
            return await _context.Set<InAppNotification>()
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Set<InAppNotification>()
                .FindAsync(notificationId);

            if (notification == null)
                return;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
