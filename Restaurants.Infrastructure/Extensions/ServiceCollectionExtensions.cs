using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Authorization.Interfaces;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Auth.Policies;
using Restaurants.Infrastructure.Auth.Policies.Requirements;
using Restaurants.Infrastructure.Auth.Policies.Requirements.Handlers;
using Restaurants.Infrastructure.Authorization.Factory;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Infrastructure.Seeders.Interfaces;

namespace Restaurants.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services,
                                         IConfiguration configuration)
    {
        // Register infrastructure services here
        services.AddDbContext<RestaurantsDbContext>(options =>
            options
                .UseSqlServer(configuration.GetConnectionString("RestaurantsDb"))
                .EnableSensitiveDataLogging()
        );

        services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>()
                .AddClaimsPrincipalFactory<RestauransUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<RestaurantsDbContext>();

        services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
        services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();

        services.AddAuthorizationBuilder()
                .AddPolicy(PolicyNames.HasNationality, builder =>
                {
                    builder.RequireClaim(AppClaimTypes.Nationality,
                                         "German",
                                         "Polish");
                })
                .AddPolicy(PolicyNames.AtLeast20, builder =>
                {
                    builder.AddRequirements(new MinimumAgeRequirement(20));
                });

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();

        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
    }
}