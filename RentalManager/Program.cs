using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RentalManager.Data;
using RentalManager.Helpers.Validations;
using RentalManager.Repositories.PropertyRepository;
using RentalManager.Services.PropertyService;
using RentalManager.Repositories.RoleRepository;
using RentalManager.Services.RoleService;
using RentalManager.Repositories.SystemCodeRepository;
using RentalManager.Services.SystemCodeService;
using RentalManager.Services.SystemCodeItemService;
using RentalManager.Repositories.SystemCodeItemRepository;
using RentalManager.Services.UnitTypeService;
using RentalManager.Repositories.UnitTypeRepository;
using RentalManager.Repositories.UnitRepository;
using RentalManager.Services.UnitService;
using RentalManager.Repositories.UtilityBillRepository;
using RentalManager.Services.UtilityBillService;
using RentalManager.Services.UserService;
using RentalManager.Repositories.UserRepository;
using RentalManager.Services.TenantService;
using RentalManager.Services;
using RentalManager.Repositories.TenantRepository;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000") // React dev server
                        .AllowAnyMethod()
                        .AllowAnyHeader());
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
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISystemCodeRepository, SystemCodeRepository>();
builder.Services.AddScoped<ISystemCodeItemRepository, SystemCodeItemRepository>();
builder.Services.AddScoped<IUnitTypeRepository, UnitTypeRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IUtilityBillRepository, UtilityBillRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();

// Register Services
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISystemCodeService, SystemCodeService>();
builder.Services.AddScoped<ISystemCodeItemService, SystemCodeItemService>();
builder.Services.AddScoped<IUnitTypeService, UnitTypeService>();
builder.Services.AddScoped<IUnitService, UnitService>();
builder.Services.AddScoped<IUtilityBillService, UtilityBillService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITenantService, TenantService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

app.UseAuthorization();

app.MapControllers();

app.Run();
