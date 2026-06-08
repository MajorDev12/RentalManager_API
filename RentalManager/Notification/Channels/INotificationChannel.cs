using RentalManager.Notification.Audience;
using RentalManager.Notification.Events;
using RentalManager.Notification.Models;
using RentalManager.Notification.Templates;

namespace RentalManager.Notification.Channels
{
    public interface INotificationChannel
    {
        string Channel { get; } // "email", "sms", "whatsapp", "inapp"
        Task SendAsync(
            INotificationEvent @event,
            NotificationPreference preference,
            INotificationTemplate template,
            NotificationRecipient recipient);
    }

}
