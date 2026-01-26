using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        // Register application services here

        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(applicationAssembly);
        });

        services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(applicationAssembly);
        });

        services.AddValidatorsFromAssembly(applicationAssembly)
                .AddFluentValidationAutoValidation();
    }
}
