using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Ambev.DeveloperEvaluation.Common.Security
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<SecuritySettings>();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                SecuritySettings settings = serviceProvider.GetRequiredService<IOptions<SecuritySettings>>().Value;
                ArgumentException.ThrowIfNullOrWhiteSpace(settings?.IssuerKey);

                var key = Encoding.UTF8.GetBytes(settings?.IssuerKey ?? string.Empty);

                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidAudience = settings?.TokenAudirence,
                        ValidIssuer = settings?.Issuer,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
                services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            }
            return services;
        }
    }
}
