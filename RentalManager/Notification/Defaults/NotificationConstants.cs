namespace RentalManager.Notification.Defaults
{
    public static class NotificationConstants
    {
        public static class Channel
        {
            public const string App = "internal";
            public const string Sms = "sms";
            public const string Email = "email";

        }

        public static class Role
        {
            public const string Owner = "Owner";
            public const string Manager = "Manager";
            public const string Landlord = "Landlord";
            public const string Tenant = "Tenant";

        }

        public static class Event
        {
            public const string TenantAdded = "TENANT_ADDED";
            public const string PaymentReceived = "PAYMENT_RECEIVED";

        }
    }
}
