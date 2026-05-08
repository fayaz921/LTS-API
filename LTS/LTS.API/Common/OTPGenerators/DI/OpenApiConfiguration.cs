using LTS.API.Infrastructure.Services.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi;
namespace LTS.API.Common.OTPGenerators.DI
{
    public static class OpenApiConfiguration
    {
        public static IServiceCollection AddOpenApiWithJwt(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new();

                    document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                    {
                        ["Bearer"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            Description = "JWT token yahan paste karo"
                        }
                    };

                    return Task.CompletedTask;
                });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            var jwtSettings = configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>()!;

            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}
