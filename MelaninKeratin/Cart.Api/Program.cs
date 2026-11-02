using BuildingBlocks.Authentication;
using Cart.Api.Behaviours;
using Cart.Api.Data;
using Cart.Api.Models;
using Carter;
using Marten;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddCarter();

var assembly = typeof(Program).Assembly;

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

//builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("cartdb")!);
    opts.Schema.For<ShopCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<ICartRepository, CartRepository>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "mk-claim");
    });
});



var app = builder.Build();

app.MapCarter();
app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");

app.Run();
