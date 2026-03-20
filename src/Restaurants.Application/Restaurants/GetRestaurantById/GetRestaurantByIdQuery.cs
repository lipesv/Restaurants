namespace Restaurants.Application.Restaurants.GetRestaurantById;

public record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDto>;

