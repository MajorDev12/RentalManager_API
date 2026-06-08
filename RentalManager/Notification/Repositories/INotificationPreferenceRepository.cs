using RentalManager.Notification.Models;

namespace RentalManager.Notification.Repositories
{
    public interface INotificationPreferenceRepository
    {
        Task<NotificationPreference?> GetPreferencesForEventAsync(string eventName, int accountId, int userId);

        Task<bool> ExistsAsync(int accountId, int userId, string eventType);
        Task AddAsync(NotificationPreference pref);

    }
}
