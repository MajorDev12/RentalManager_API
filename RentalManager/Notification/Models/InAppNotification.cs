namespace RentalManager.Notification.Models
{
    public class InAppNotification
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }

        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
