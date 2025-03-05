using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Cache;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.NoSql.MDb;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting web application");
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"))
                .SetApplicationName("AmbevDeveloperEvaluation");


            var appsettings = $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development" }.json";
            builder.Configuration
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile(appsettings, optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("*") // Permite chamadas do Angular
                          .AllowAnyMethod() // Permite qualquer método (GET, POST, PUT, DELETE, etc.)
                          .AllowAnyHeader(); // Permite qualquer cabeçalho
                                             //.AllowCredentials(); // Permite envio de credenciais (cookies, autenticação)
                });
            });


            builder.AddDefaultLogging();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.AddBasicHealthChecks();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
                string caminhoAplicacao = AppDomain.CurrentDomain.BaseDirectory;
                string nomeAplicacao = AppDomain.CurrentDomain.FriendlyName;
                string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");
                c.IncludeXmlComments(caminhoXmlDoc);
            });

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

            // Security settings
            builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection("Jwt"));
            builder.Services.AddJwtAuthentication(builder.Configuration);

            builder.RegisterDependencies();

            builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(ApplicationLayer).Assembly);

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    typeof(ApplicationLayer).Assembly,
                    typeof(Program).Assembly
                );
            });

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Add Settings Redis
            builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("Redis"));

            // Add Setting Mongodb
            builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));


            // Authorization service, policy
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyManager", policy => policy.RequireAssertion(h =>
                    h.User.FindFirstValue(ClaimTypes.Role).Contains("Manager"))
                );
                options.AddPolicy("OnlyAdmin", policy => policy.RequireAssertion(h =>
                    h.User.FindFirstValue(ClaimTypes.Role).Contains("Admin"))
                );
                options.AddPolicy("OnlyCustomer", policy => policy.RequireAssertion(h =>
                    h.User.FindFirstValue(ClaimTypes.Role).Contains("Customer"))
                );

                // Mesclar role na policy
                options.AddPolicy("OnlyManagerOrAdmin", policy => policy.RequireAssertion(h =>
                    h.User.FindFirstValue(ClaimTypes.Role).Contains("Manager")
                    || h.User.FindFirstValue(ClaimTypes.Role).Contains("Admin")
                ));
                options.AddPolicy("OnlyAdminOrCustumer", policy => policy.RequireAssertion(h =>
                    h.User.FindFirstValue(ClaimTypes.Role).Contains("Admin")
                    || h.User.FindFirstValue(ClaimTypes.Role).Contains("Customer")
                ));
                options.AddPolicy("OnlyManagerOrCustomer", policy => policy.RequireAssertion(h =>
                    h.User.FindFirstValue(ClaimTypes.Role).Contains("Manager")
                    || h.User.FindFirstValue(ClaimTypes.Role).Contains("Customer")
                ));
                options.AddPolicy("AllManagerOrAdminOrCustomer", policy => policy.RequireAssertion(h =>
                    h.User.FindFirstValue(ClaimTypes.Role).Contains("Manager")
                    || h.User.FindFirstValue(ClaimTypes.Role).Contains("Admin")
                    || h.User.FindFirstValue(ClaimTypes.Role).Contains("Customer")
                ));
            });

           

            var app = builder.Build();
            app.UseCors("AllowSpecificOrigin");
            app.UseMiddleware<InterceptExceptionMiddleware>();

            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseBasicHealthChecks();
            app.MapControllers();
            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Application terminated unexpectedly : {ex.Message}");
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
