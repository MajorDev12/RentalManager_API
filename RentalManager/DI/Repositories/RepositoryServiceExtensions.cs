using Microsoft.Extensions.DependencyInjection;
using RentalManager.Notification.Repositories;
using RentalManager.Repositories.AuditTrailRepository;
using RentalManager.Repositories.ExpenseRepository;
using RentalManager.Repositories.HomeRepository;
using RentalManager.Repositories.MeterReadingRepository;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Repositories.ReportRepository;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Repositories.SystemCodeRepository;
using RentalManager.Repositories.TenantRepository;
using RentalManager.Repositories.TokenRepository;
using RentalManager.Repositories.TransactionRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Repositories.UnitTypeRepository;
using RentalManager.Repositories.UserRepository;
using RentalManager.Repositories.UtilityBillRepository;

namespace RentalManager.DI.Repositories
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositoryServices(
            this IServiceCollection services)
        {
            // Core domain repositories
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISystemCodeRepository, SystemCodeRepository>();
            services.AddScoped<ISystemCodeItemRepository, SystemCodeItemRepository>();
            services.AddScoped<IUnitTypeRepository, UnitTypeRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IUtilityBillRepository, UtilityBillRepository>();
            services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();

            // Notification repositories
            

            return services;
        }
    }
}