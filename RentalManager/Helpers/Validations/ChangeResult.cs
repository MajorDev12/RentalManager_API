namespace RentalManager.Helpers.Validations
{
    public class ChangeResult
    {
        public bool HasChanges => Changes.Count > 0;

        public Dictionary<string, (object? OldValue, object? NewValue)> Changes { get; set; }
            = new();
    }
}
