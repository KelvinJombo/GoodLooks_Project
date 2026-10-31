using Microsoft.EntityFrameworkCore;
using Payment.Api.Data;
using Payment.Api.Options;
using Payment.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

var connectionString =
    builder.Configuration.GetConnectionString("paymentdb")
    ?? throw new InvalidOperationException(
        "Connection string 'paymentdb' was not found.");

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services
    .AddOptions<PaystackOptions>().Bind(builder.Configuration.GetSection(PaystackOptions.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.SecretKey), "Paystack secret key is required.")
    .Validate(options => Uri.TryCreate(
            options.CallbackUrl,
            UriKind.Absolute,
            out var uri)
            && uri.Scheme == Uri.UriSchemeHttps,
        "Paystack callback URL must be an HTTPS URL.")
    .ValidateOnStart();

builder.Services.AddHttpClient<IPaystackClient, PaystackClient>(
    client =>
    {
        client.BaseAddress = new Uri("https://api.paystack.co/");
        client.Timeout = TimeSpan.FromSeconds(30);
    });

builder.Services.AddScoped<IPaymentService, PaymentService>();


//builder.Services.AddScoped<IOrderPaymentLookup, OrderPaymentLookup>();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
