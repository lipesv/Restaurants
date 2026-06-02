using Restaurants.API.Extensions;
using Restaurants.Application.Extensions;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Infrastructure.Extensions;
using Restaurants.Infrastructure.Seeders.Interfaces;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddPresentation();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimeLoggingMiddleware>();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAll");

    app.UseHttpsRedirection();

    app.MapGroup("api/identity")
       .WithTags("Identity")
       .MapIdentityApi<User>();

    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();
        await seeder.SeedAsync();
    }

    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }