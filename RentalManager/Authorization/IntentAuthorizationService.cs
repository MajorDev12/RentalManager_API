namespace RentalManager.Authorization
{
    public class IntentAuthorizationService
    {
        public bool IsAllowed(string role, string intent)
        {
            return RoleIntentMap.Map
                .TryGetValue(role, out var intents)
                && intents.Contains(intent);
        }
    }

}
