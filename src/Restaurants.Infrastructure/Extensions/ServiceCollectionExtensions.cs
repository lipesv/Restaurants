using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Authorization.Factory;
using Restaurants.Infrastructure.Authorization.Policies.Requirements.Handlers;
using Restaurants.Infrastructure.Authorization.Policies.Requirements;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

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
                })
                .AddPolicy(PolicyNames.MultiOwnerPolicy, builder =>
                {
                    builder.AddRequirements(new MinimumRestaurantsRequirement(2));
                });

        services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, MinimumRestaurantsRequirementHandler>();

        services.AddScoped<IRestaurantAuthorizationService, RestaurantAuthorizationService>();
    }
}