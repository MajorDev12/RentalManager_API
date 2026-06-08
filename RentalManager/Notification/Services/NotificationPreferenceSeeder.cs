using RentalManager.Notification.Defaults;
using RentalManager.Notification.Models;
using RentalManager.Notification.Repositories;

namespace RentalManager.Notification.Services
{
    public class NotificationPreferenceSeeder
    {
        private readonly INotificationPreferenceRepository _repo;

        public NotificationPreferenceSeeder(
            INotificationPreferenceRepository repo)
        {
            _repo = repo;
        }

        public async Task SeedForUserAsync(
            int accountId,
            int userId,
            string role)
        {
            var defaults = NotificationDefaults.All
                .Where(d => d.Role.Equals(role, StringComparison.OrdinalIgnoreCase));

            foreach (var d in defaults)
            {
                var exists = await _repo.ExistsAsync(
                    accountId,
                    userId,
                    d.EventType);

                if (exists)
                    continue;

                var pref = new NotificationPreference
                {
                    AccountId = accountId,
                    UserId = userId,
                    EventType = d.EventType,
                    InAppEnabled = d.InAppEnabled,
                    SmsEnabled = d.SmsEnabled,
                    EmailEnabled = d.EmailEnabled,
                    IsEnabled = true
                };

                await _repo.AddAsync(pref);
            }
        }
    }
}
