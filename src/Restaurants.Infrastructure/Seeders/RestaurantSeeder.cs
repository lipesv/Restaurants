using Restaurants.Domain.Constants;

namespace Restaurants.Infrastructure.Seeders;

internal class DatabaseSeeder(RestaurantsDbContext dbContext,
                              RoleManager<IdentityRole> roleManager,
                              UserManager<User> userManager) : IDatabaseSeeder
{
    public async Task SeedAsync()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (!await dbContext.Database.CanConnectAsync())
        {
            return;
        }

        await SeedRolesAsync();

        var owner = await SeedOwnerUserAsync();

        await SeedAdminUserAsync();

        await SeedDefaultUserAsync();

        await SeedRestaurantsAsync(owner.Id);
    }

    private async Task SeedRolesAsync()
    {
        string[] roles =
        [
            UserRoles.User,
            UserRoles.Owner,
            UserRoles.Admin
        ];

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            var result = await roleManager.CreateAsync(new IdentityRole(role));

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Erro ao criar role '{role}': {errors}");
            }
        }
    }

    private Task<User> SeedOwnerUserAsync()
    {
        return EnsureUserWithRole(
            email: "owner@restaurants.example",
            password: "Owner123!",
            role: UserRoles.Owner);
    }

    private Task<User> SeedAdminUserAsync()
    {
        return EnsureUserWithRole(
            email: "admin@restaurants.example",
            password: "Admin123!",
            role: UserRoles.Admin);
    }

    private Task<User> SeedDefaultUserAsync()
    {
        return EnsureUserWithRole(
            email: "user@restaurants.example",
            password: "User123!",
            role: UserRoles.User);
    }

    private async Task<User> EnsureUserWithRole(string email,
                                                string password,
                                                string role)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    $"Erro ao criar usuário '{email}': {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            var roleResult = await userManager.AddToRoleAsync(user, role);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    $"Erro ao associar '{email}' à role '{role}': {errors}");
            }
        }

        return user;
    }

    private async Task SeedRestaurantsAsync(string ownerId)
    {
        if (await dbContext.Restaurants.AnyAsync())
            return;

        var restaurants = GetRestaurants(ownerId);

        await dbContext.Restaurants.AddRangeAsync(restaurants);
        await dbContext.SaveChangesAsync();
    }

    private static List<Restaurant> GetRestaurants(string ownerId)
    {
        return
        [
            new()
            {
                Name = "Pasta Palace",
                Description = "Handmade pasta and sauces inspired by traditional Italian recipes.",
                Category = "Italian",
                HasDelivery = true,
                ContactEmail = "contact@pastapalace.example",
                ContactNumber = "+1-555-0101",
                OwnerId = ownerId,
                Address = new()
                {
                    City = "Springfield",
                    Street = "12 Olive Way",
                    PostalCode = "12345"
                }
            },

            new()
            {
                Name = "Sushi Central",
                Description = "Modern sushi bar using sustainably sourced fish.",
                Category = "Japanese",
                HasDelivery = false,
                ContactEmail = "hello@sushicentral.example",
                ContactNumber = "+1-555-0202",
                OwnerId = ownerId,
                Address = new()
                {
                    City = "Riverton",
                    Street = "88 Harbor Road",
                    PostalCode = "67890"
                }
            }
        ];
    }
}