using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RentalManager.Authorization;
using RentalManager.Authorization.Handlers;
using RentalManager.Data;
using RentalManager.Helpers.Validations;
using RentalManager.Middlewares;
using RentalManager.Models;
using RentalManager.Repositories.ExpenseRepository;
using RentalManager.Repositories.HomeRepository;
using RentalManager.Repositories.InvoiceLineRepository;
using RentalManager.Repositories.InvoiceRepository;
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
using RentalManager.Services;
using RentalManager.Services.AccountAccessService;
using RentalManager.Services.AuthService;
using RentalManager.Services.ExpenseService;
using RentalManager.Services.HomeService;
using RentalManager.Services.InvoiceService;
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
using System.Security.Cryptography;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000") // React dev server
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
});

builder.Services.Configure<ApiBehaviorOptions>(options => 
{
    options.SuppressModelStateInvalidFilter = true;
});



// Add Filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelStateValidator>();
});


// Register Repositories
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISystemCodeRepository, SystemCodeRepository>();
builder.Services.AddScoped<ISystemCodeItemRepository, SystemCodeItemRepository>();
builder.Services.AddScoped<IUnitTypeRepository, UnitTypeRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IUtilityBillRepository, UtilityBillRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceLineRepository, InvoiceLineRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

// Register Services
builder.Services.AddScoped<IHomeService, HomeService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISystemCodeService, SystemCodeService>();
builder.Services.AddScoped<ISystemCodeItemService, SystemCodeItemService>();
builder.Services.AddScoped<IUnitTypeService, UnitTypeService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IUtilityBillService, UtilityBillService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountAccessService, AccountAccessService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    // password policy
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();



// Add authentication with JwtBearer (validate using public key)
var publicKeyPath = Path.Combine(builder.Environment.ContentRootPath, "Keys", "public.pem");

if (!File.Exists(publicKeyPath))
    throw new Exception("Public key file not found");

var publicPem = File.ReadAllText(publicKeyPath);

// Create RSA from PEM
var publicRsa = RSA.Create();
publicRsa.ImportFromPem(publicPem.ToCharArray());

// Create signing key
var publicKey = new RsaSecurityKey(publicRsa)
{
    KeyId = "rsa-key-2025-01" // MUST match TokenService
};



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30),

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = publicKey,

        RequireSignedTokens = true,
        RequireExpirationTime = true

    };
});



builder.Services.AddAppAuthorization();

builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<IAccountContext>(sp =>
{
    var http = sp.GetRequiredService<IHttpContextAccessor>();
    var claim = http.HttpContext?.User?.FindFirst("accountId")?.Value;

    return new AccountContext
    {
        AccountId = int.TryParse(claim, out var id) ? id : 0
    };
});

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, ApiAuthorizationResultHandler>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<AccountAccessMiddleware>();

app.MapControllers();

app.Run();
