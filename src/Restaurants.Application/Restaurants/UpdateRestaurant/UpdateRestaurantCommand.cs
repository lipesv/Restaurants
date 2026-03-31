namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public record UpdateRestaurantCommand(int Id,
                                      string Name,
                                      string Description,
                                      bool HasDelivery) : IRequest;
