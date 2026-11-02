using BuildingBlocks.Authentication;
using Catalog.Application.Interfaces.IGenericRepository;
using Catalog.Application.Interfaces.IService;
using Catalog.Application.ServicesImplementations;
using Catalog.Application.Validators;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repository.GenericRepository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();

var connectionString =
    builder.Configuration.GetConnectionString("CatalogDb") ?? throw new InvalidOperationException(
        "Connection string 'CatalogDb' was not found.");


builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseNpgsql(connectionString));



builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "mk-claim");
    });
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IServiService, ServiService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(ICloudinaryServices<>), typeof(CloudinaryServices<>));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
