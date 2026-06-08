using RentalManager.Notification.Models;

namespace RentalManager.Notification.Repositories
{
    public interface INotificationRepository
    {
        Task AddAsync(InAppNotification notification);

        Task<IReadOnlyList<InAppNotification>> GetUnreadAsync(int userId);

        Task<IReadOnlyList<InAppNotification>> GetAllAsync(int userId);

        Task MarkAsReadAsync(int notificationId);
    }
}
