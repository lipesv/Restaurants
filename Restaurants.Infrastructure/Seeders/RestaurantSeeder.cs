using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Seeders.Interfaces;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants =
        [
            new() {
                Name = "Pasta Palace",
                Description = "Handmade pasta and sauces inspired by traditional Italian recipes.",
                Category = "Italian",
                HasDelivery = true,
                ContactEmail = "contact@pastapalace.example",
                ContactNumber = "+1-555-0101",
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
                    },
                    new()
                    {
                        Name = "Cacio e Pepe",
                        Description = "Pecorino, black pepper and perfectly al dente spaghetti.",
                        Price = 12.00m
                    },
                    new()
                    {
                        Name = "Tiramisu",
                        Description = "Classic espresso-soaked ladyfingers with mascarpone.",
                        Price = 7.00m
                    }
                ]
            },

            new() {
                Name = "Sushi Central",
                Description = "Modern sushi bar using sustainably sourced fish and seasonal produce.",
                Category = "Japanese",
                HasDelivery = false,
                ContactEmail = "hello@sushicentral.example",
                ContactNumber = "+1-555-0202",
                Address = new()
                {
                    City = "Riverton",
                    Street = "88 Harbor Road",
                    PostalCode = "67890"
                },
                Dishes =
                [
                    new()
                    {
                        Name = "Salmon Nigiri (2 pcs)",
                        Description = "Fresh Atlantic salmon over vinegared rice.",
                        Price = 6.50m
                    },
                    new()
                    {
                        Name = "Spicy Tuna Roll",
                        Description = "Tuna with house spicy mayo and cucumber.",
                        Price = 9.00m
                    },
                    new()
                    {
                        Name = "Miso Soup",
                        Description = "Traditional miso with wakame and tofu.",
                        Price = 3.50m
                    }
                ]
            },

            new() {
                Name = "Burger Barn",
                Description = "Classic and gourmet burgers, hand-cut fries and craft sodas.",
                Category = "American",
                HasDelivery = true,
                ContactEmail = "order@burgerbarn.example",
                ContactNumber = "+1-555-0303",
                Address = new()
                {
                    City = "Lakeside",
                    Street = "450 Main Street",
                    PostalCode = "24680"
                },
                Dishes =
                [
                    new()
                    {
                        Name = "Classic Cheeseburger",
                        Description = "Grilled patty, cheddar, lettuce, tomato, pickles.",
                        Price = 10.00m
                    },
                    new()

                    {
                        Name = "BBQ Bacon Burger",
                        Description = "Smoky BBQ sauce, bacon, crispy onions.",
                        Price = 12.50m
                    },
                    new()
                    {
                        Name = "Sweet Potato Fries",
                        Description = "Crispy sweet potato fries with herb salt.",
                        Price = 4.50m
                    }
                ]
            },

            new() {
                Name = "Green Garden",
                Description = "Plant-forward dishes focusing on local and seasonal produce.",
                Category = "Vegetarian",
                HasDelivery = false,
                ContactEmail = "info@greengarden.example",
                ContactNumber = "+1-555-0404",
                Address = new()
                {
                    City = "Meadowbrook",
                    Street = "221 Garden Lane",
                    PostalCode = "11223"
                },
                Dishes =
                [
                    new()
                    {
                        Name = "Roasted Beet Salad",
                        Description = "Mixed greens, roasted beets, citrus vinaigrette.",
                        Price = 11.00m
                    },
                    new()
                    {
                        Name = "Mushroom Risotto",
                        Description = "Creamy arborio with wild mushrooms and herbs.",
                        Price = 13.75m
                    },
                    new()
                    {
                        Name = "Lemon Tahini Tart",
                        Description = "Zesty citrus tart with tahini crust.",
                        Price = 6.50m
                    }
                ]
            }
        ];

        return restaurants;
    }
}
