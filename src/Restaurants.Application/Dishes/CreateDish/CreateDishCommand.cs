using MediatR;

namespace Restaurants.Application.Dishes.CreateDish;

public record CreateDishCommand(decimal Price,
                                int? KiloCalories) : IRequest<int>
{
    public int RestaurantId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}

