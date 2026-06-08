
namespace RentalManager.BusinessRules.Core
{

    public class RuleEngine : IRuleEngine
    {
        private readonly IServiceProvider _serviceProvider;

        public RuleEngine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ValidateAsync<T>(T entity, RuleOperation operation)
        {
            var rules = _serviceProvider.GetServices<IBusinessRule<T>>();

            foreach (var rule in rules)
            {
                await rule.ValidateAsync(entity, operation);
            }
        }
    }
}
