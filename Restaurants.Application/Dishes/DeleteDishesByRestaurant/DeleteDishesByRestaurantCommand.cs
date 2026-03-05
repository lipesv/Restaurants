using MediatR;
namespace Restaurants.Application.Dishes.DeleteDishesByRestaurant;

public record DeleteDishesByRestaurantCommand(int restaurantId) : IRequest;
