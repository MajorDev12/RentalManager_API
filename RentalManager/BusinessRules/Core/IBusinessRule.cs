namespace RentalManager.BusinessRules.Core
{
    public interface IBusinessRule<T>
    {
        Task ValidateAsync(T entity, RuleOperation operation);
    }
}
