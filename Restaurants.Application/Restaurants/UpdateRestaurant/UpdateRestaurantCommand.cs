using MediatR;

namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public record UpdateRestaurantCommand(int Id, bool HasDelivery) : IRequest<bool>
{
    public UpdateRestaurantCommand() : this(0, false) { }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
