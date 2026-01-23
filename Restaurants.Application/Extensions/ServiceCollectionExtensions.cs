using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Services;
using Restaurants.Application.Services.Interfaces;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        // Register application services here

        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddScoped<IRestaurantsService, RestaurantsService>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(applicationAssembly);
        });

        services.AddValidatorsFromAssembly(applicationAssembly)
                .AddFluentValidationAutoValidation();
    }
}
