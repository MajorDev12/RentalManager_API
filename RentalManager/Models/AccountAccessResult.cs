namespace RentalManager.Models
{
    public class AccountAccessResult
    {
        public bool Allowed { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
