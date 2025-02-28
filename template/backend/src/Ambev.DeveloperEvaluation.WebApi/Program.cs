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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using StackExchange.Redis;
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
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                )
            );

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

            if (app.Environment.IsDevelopment())
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
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
