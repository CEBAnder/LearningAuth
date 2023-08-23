using System.Threading.RateLimiting;
using FluentMigrator.Runner;
using LearningAuth.Contracts.Shared;
using LearningAuth.Data;
using LearningAuth.Data.Migrations;
using LearningAuth.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;

namespace LearningAuth.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(connectionString);
        }
        
        services.AddSingleton<DbContext>();
        services.AddSingleton<Database>();
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(Migration_1_AddUserTable).Assembly).For.Migrations());

        return services;
    }
    
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                })
            .AddCookie();
        services
            .AddAuthorization(
                options =>
                {
                    options.AddPolicy(
                        Constants.AuthenticationPolicies.CookieAdmin,
                        policy =>
                        {
                            policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
                            policy.Requirements.Add(
                                new RolesAuthorizationRequirement(new List<string> { Role.Admin.ToString() }));
                        });
                });

        return services;
    }

    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return new ValueTask();
            };
            options.AddFixedWindowLimiter(Constants.RateLimiterPolicies.FixedWindow, limiterOptions =>
            {
                limiterOptions.PermitLimit = 4;
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.Window = TimeSpan.FromSeconds(30);
            });
        });

        return services;
    }
}