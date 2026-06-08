using RentalManager.DI.Core;
using RentalManager.DI.Seeding;

var builder = WebApplication.CreateBuilder(args);

//SERVICE REGISTRATION
builder.Services.AddApplicationServices(
    builder.Configuration,
    builder.Environment
);

var app = builder.Build();

// DATABASE SEEDING
await app.Services.SeedDatabaseAsync();


// MIDDLEWARE PIPELINE

app.UseAppMiddlewares();

app.MapControllers();

app.Run();
