namespace RentalManager.BusinessRules.Core
{
    public interface IRuleEngine
    {
        Task ValidateAsync<T>(T entity, RuleOperation operation);
    }
}
