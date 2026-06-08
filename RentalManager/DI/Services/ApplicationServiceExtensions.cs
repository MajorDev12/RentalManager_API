using Microsoft.Extensions.DependencyInjection;
using RentalManager.Services;
using RentalManager.Services.AccountAccessService;
using RentalManager.Services.AuditTrailService;
using RentalManager.Services.AuthService;
using RentalManager.Services.ExpenseService;
using RentalManager.Services.HomeService;
using RentalManager.Services.MeterReadingService;
using RentalManager.Services.PropertyService;
using RentalManager.Services.ReportService;
using RentalManager.Services.RoleService;
using RentalManager.Services.SystemCodeItemService;
using RentalManager.Services.SystemCodeService;
using RentalManager.Services.TenantService;
using RentalManager.Services.TokenService;
using RentalManager.Services.TransactionService;
using RentalManager.Services.UnitService;
using RentalManager.Services.UnitTypeService;
using RentalManager.Services.UserService;
using RentalManager.Services.UtilityBillService;
using RentalManager.Services.WhatsAppService;

namespace RentalManager.DI.Services
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddBusinessServices(
            this IServiceCollection services)
        {
            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ISystemCodeService, SystemCodeService>();
            services.AddScoped<ISystemCodeItemService, SystemCodeItemService>();
            services.AddScoped<IUnitTypeService, UnitTypeService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IUtilityBillService, UtilityBillService>();
            services.AddScoped<IMeterReadingService, MeterReadingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountAccessService, AccountAccessService>();
            services.AddScoped<IWhatsAppService, WhatsAppService>();
            services.AddScoped<IAuditTrailService, AuditTrailService>();

            return services;
        }
    }
}