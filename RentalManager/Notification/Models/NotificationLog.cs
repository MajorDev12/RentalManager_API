namespace RentalManager.Notification.Models
{
    public class NotificationLog
    {
        public string Channel { get; set; }
        public string EventName { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public DateTime SentAt { get; set; }
    }

}
