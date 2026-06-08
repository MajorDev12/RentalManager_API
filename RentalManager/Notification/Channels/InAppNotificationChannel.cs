using Microsoft.EntityFrameworkCore;
using RentalManager.Data;
using RentalManager.Notification.Audience;
using RentalManager.Notification.Defaults;
using RentalManager.Notification.Events;
using RentalManager.Notification.Models;
using RentalManager.Notification.Templates;
using System.Text.Json;

namespace RentalManager.Notification.Channels
{
    public class InAppNotificationChannel : INotificationChannel
    {
        public string Channel => NotificationConstants.Channel.App;

        private readonly ApplicationDbContext _context;

        public InAppNotificationChannel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendAsync(
            INotificationEvent @event,
            NotificationPreference pref,
            INotificationTemplate template,
            NotificationRecipient recipient)
        {
            var notification = new InAppNotification
            {
                UserId = pref.UserId ?? 0,
                Title = template.BuildTitle(@event),
                Body = template.BuildBody(@event),
                IsRead = false
            };

            // Save to DB
            _context.InAppNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }
    }

}
