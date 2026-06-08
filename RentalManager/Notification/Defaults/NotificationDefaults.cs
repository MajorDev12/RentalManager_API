using RentalManager.Notification.Events;

namespace RentalManager.Notification.Defaults
{
    public static class NotificationDefaults
    {
        public static readonly IReadOnlyList<NotificationDefault> All =
            new List<NotificationDefault>
            {
                // TENANT ADDED
                new()
                {
                    EventType = NotificationEventNames.TenantAdded,
                    Role = NotificationConstants.Role.Owner,
                    InAppEnabled = true
                },
                new()
                {
                    EventType = NotificationEventNames.TenantAdded,
                    Role = NotificationConstants.Role.Manager,
                    InAppEnabled = true
                },
                new()
                {
                    EventType = NotificationEventNames.TenantAdded,
                    Role = NotificationConstants.Role.Landlord,
                    InAppEnabled = true
                },
                new()
                {
                    EventType = NotificationEventNames.TenantAdded,
                    Role = NotificationConstants.Role.Tenant,
                    InAppEnabled = true
                },

                // PAYMENT RECEIVED
                new()
                {
                    EventType = NotificationEventNames.PaymentReceived,
                    Role = NotificationConstants.Role.Owner,
                    InAppEnabled = true,
                    SmsEnabled = true
                }
            };
    }
}
