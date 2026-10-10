using Identity.Api.Data;
using Identity.Api.Models;
using Identity.Api.Repository;
using Identity.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("identitydb") ?? throw new InvalidOperationException(
        "Connection string 'identitydb' was not found.");

builder.AddServiceDefaults();

// ASP.NET Identity database.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register ASP.NET Identity before AddIdentityServer().
builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Duende IdentityServer.
var migrationsAssembly =
    typeof(Program).Assembly.GetName().Name!;

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
.AddAspNetIdentity<User>().AddConfigurationStore(options =>
{
    options.ConfigureDbContext = db =>
        db.UseNpgsql(connectionString, sql =>
            sql.MigrationsAssembly(migrationsAssembly));
})
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = db =>
        db.UseNpgsql(connectionString, sql =>
            sql.MigrationsAssembly(migrationsAssembly));
})
.AddDeveloperSigningCredential();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequiredLength = 12;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan =
        TimeSpan.FromMinutes(15);
});


builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// IdentityServer includes the required authentication
// middleware as part of its pipeline integration.
app.UseIdentityServer();

app.UseAuthorization();

app.MapControllers();

await Seeder.SeedAllAsync(app.Services);

app.Run();