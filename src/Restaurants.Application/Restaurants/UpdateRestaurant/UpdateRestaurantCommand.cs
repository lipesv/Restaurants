using MediatR;

namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public record UpdateRestaurantCommand(int Id, bool HasDelivery) : IRequest
{
    public int Id { get; set; } = Id;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
}
