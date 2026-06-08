using Microsoft.AspNetCore.Authorization;
using RentalManager.Authorization.Handlers;
using RentalManager.Authorization.Policies;

namespace RentalManager.Authorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                PropertyPolicies.Register(options);
                UtilityBillPolicies.Register(options);
                UnitTypePolicies.Register(options);
                UnitPolicies.Register(options);
                TenantPolicies.Register(options);
                TransactionPolicies.Register(options);
                ExpensePolicies.Register(options);
                ReportPolicies.Register(options);
                NotificationPolicies.Register(options);
            });


            return services;
        }
    }
}
