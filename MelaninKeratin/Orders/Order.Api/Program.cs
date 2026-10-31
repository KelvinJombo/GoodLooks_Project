using Microsoft.EntityFrameworkCore;
using Order.Api;
using Order.Application;
using Order.Infrastructure;
using Order.Infrastructure.Data;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//    options.Authority = "https://localhost:7031";

//          options.Authority = builder.Configuration["Identity:Authority"];

//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateAudience = false
//        };
//    });

// Add services to the container.



builder.Services.AddApplicationServices().AddInfrastructureServices(builder.Configuration).AddApiServices();

var app = builder.Build();



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();



app.Run();
                