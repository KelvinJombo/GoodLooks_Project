using Identity.Api.Data;
using Identity.Api.Models;
using Identity.Api.Repository;
using Identity.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register ASP.NET Identity before AddIdentityServer().
builder.Services
    .AddIdentity<User, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


var connectionString =
    builder.Configuration.GetConnectionString("identitydb") ?? throw new InvalidOperationException(
        "Connection string 'identitydb' was not found.");

builder.AddServiceDefaults();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.UserInteraction.LoginUrl = "/account/login";
})
    .AddAspNetIdentity<User>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = db =>
            db.UseNpgsql(
                connectionString,
                sql =>
                {
                    sql.MigrationsAssembly(
                        typeof(Program).Assembly.GetName().Name);

                    sql.MigrationsHistoryTable(
                        "__EFMigrationsHistory_Configuration");
                });
    })
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = db =>
        db.UseNpgsql(
            connectionString,
            sql =>
            {
                sql.MigrationsAssembly(
                    typeof(Program).Assembly.GetName().Name);

                sql.MigrationsHistoryTable(
                    "__EFMigrationsHistory_Operational");
            });
}).AddDeveloperSigningCredential();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "MelaninKeratin.Identity";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;

    options.LoginPath = "/account/login";
    options.LogoutPath = "/account/logout";

    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

builder.Services.ConfigureExternalCookie(options =>
{
    options.Cookie.Name = "MelaninKeratin.External";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;

    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});


builder.Services
    .AddAuthentication()
    .AddGoogle(options =>
    {
        options.SignInScheme = IdentityConstants.ExternalScheme;

        options.ClientId =
            builder.Configuration["Authentication:Google:ClientId"]
            ?? throw new InvalidOperationException(
                "Google ClientId is not configured.");

        options.ClientSecret =
            builder.Configuration["Authentication:Google:ClientSecret"]
            ?? throw new InvalidOperationException(
                "Google ClientSecret is not configured.");

        options.Scope.Add("email");
    })
    .AddFacebook(options =>
    {
        options.SignInScheme = IdentityConstants.ExternalScheme;

        options.AppId =
            builder.Configuration["Authentication:Facebook:AppId"]
            ?? throw new InvalidOperationException(
                "Facebook AppId is not configured.");

        options.AppSecret =
            builder.Configuration["Authentication:Facebook:AppSecret"]
            ?? throw new InvalidOperationException(
                "Facebook AppSecret is not configured.");

        options.Fields.Add("email");
        options.Fields.Add("first_name");
        options.Fields.Add("last_name");
    });

builder.Services.AddRazorPages();


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
builder.Services.AddScoped<IExternalLoginService, ExternalLoginService>();
builder.Services.AddScoped<IIdentityEmailService, DevelopmentIdentityEmailService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

// IdentityServer includes the required authentication
// middleware as part of its pipeline integration.
app.UseIdentityServer();

app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

await Seeder.SeedAllAsync(app.Services);

app.Run();