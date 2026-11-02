using BuildingBlocks.Authentication;
using Order.Api;
using Order.Application;
using Order.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddJwtAuthentication(builder.Configuration);

//Add services to the container.



builder.Services.AddApplicationServices().AddInfrastructureServices(builder.Configuration).AddApiServices();

var app = builder.Build();



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();



app.Run();
                