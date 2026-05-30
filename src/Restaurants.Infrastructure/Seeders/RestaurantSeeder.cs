using Restaurants.Domain.Constants;

namespace Restaurants.Infrastructure.Seeders;

internal class DatabaseSeeder(
    RestaurantsDbContext dbContext,
    RoleManager<IdentityRole> roleManager,
    UserManager<User> userManager) : IDatabaseSeeder
{
    public async Task SeedAsync()
    {
        if (!await dbContext.Database.CanConnectAsync())
        {
            return;
        }

        await SeedRolesAsync();

        var owner = await SeedOwnerUserAsync();

        await SeedRestaurantsAsync(owner.Id);
    }

    // =========================
    // ROLES
    // =========================
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
            {
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole(role));

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Erro ao criar role '{role}': {errors}");
            }
        }
    }

    // =========================
    // USER OWNER
    // =========================
    private async Task<User> SeedOwnerUserAsync()
    {
        const string ownerEmail = "owner@restaurants.example";
        const string ownerPassword = "Owner123!";

        var owner = await userManager.FindByEmailAsync(ownerEmail);

        if (owner is null)
        {
            owner = new User
            {
                UserName = ownerEmail,
                Email = ownerEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(owner, ownerPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    $"Erro ao criar usuário owner seed: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(owner, UserRoles.Owner))
        {
            var addToRoleResult = await userManager.AddToRoleAsync(owner, UserRoles.Owner);

            if (!addToRoleResult.Succeeded)
            {
                var errors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                throw new InvalidOperationException(
                    $"Erro ao associar usuário à role Owner: {errors}");
            }
        }

        return owner;
    }

    // =========================
    // RESTAURANTS
    // =========================
    private async Task SeedRestaurantsAsync(string ownerId)
    {
        if (await dbContext.Restaurants.AnyAsync())
        {
            return;
        }

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
                },
                Dishes =
                [
                    new()
                    {
                        Name = "Tagliatelle Bolognese",
                        Description = "Slow-cooked beef ragù with fresh tagliatelle.",
                        Price = 14.50m
                    }
                ]
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