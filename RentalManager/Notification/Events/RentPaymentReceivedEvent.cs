namespace RentalManager.Notification.Events
{
    public record RentPaymentReceivedEvent(
    int AccountId,
    int TenantId,
    decimal Amount
) : INotificationEvent
    {
        public string EventName => "rent_payment_received";

        public Dictionary<string, object> Data => new()
        {
            ["TenantId"] = TenantId,
            ["Amount"] = Amount
        };
    }

}
