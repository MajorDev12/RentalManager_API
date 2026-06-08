using Microsoft.Extensions.DependencyInjection;
using RentalManager.BusinessRules.Core;
using RentalManager.BusinessRules.Utilities;
using RentalManager.Models;

namespace RentalManager.DI.BusinessRules
{
    public static class BusinessRuleServiceCollection
    {
        public static IServiceCollection AddBusinessRules(this IServiceCollection services)
        {
            // Core engine
            services.AddScoped<IRuleEngine, RuleEngine>();

            // Property Utility rules
            services.AddScoped<IBusinessRule<UtilityBill>, PropertyUtilityUniqueRule>();

            return services;
        }
    }
}