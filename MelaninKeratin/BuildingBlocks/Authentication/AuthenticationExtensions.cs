using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authority = configuration["Identity:Authority"]
            ?? throw new InvalidOperationException(
                "Identity Authority is not configured.");

        services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
            });

        services.AddAuthorization();

        return services;
    }
}