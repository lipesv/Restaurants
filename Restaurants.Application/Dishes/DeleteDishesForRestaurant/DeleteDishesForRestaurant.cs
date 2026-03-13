using MediatR;
namespace Restaurants.Application.Dishes.DeleteDishesForRestaurant;

public record DeleteDishesForRestaurant(int restaurantId) : IRequest;
