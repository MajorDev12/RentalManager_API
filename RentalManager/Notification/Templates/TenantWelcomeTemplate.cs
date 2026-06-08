using RentalManager.Notification.Events;

namespace RentalManager.Notification.Templates
{
    public class TenantWelcomeTemplate : INotificationTemplate
    {
        public string EventName => NotificationEventNames.TenantAdded;
        public string Role => "Tenant";

        public string BuildTitle(INotificationEvent e)
            => "Welcome 🎉";

        public string BuildBody(INotificationEvent e)
            => "Welcome to your new home! We're glad to have you.";
    }

}
