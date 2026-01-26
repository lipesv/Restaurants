using MediatR;

namespace Restaurants.Application.Restaurants.DeleteRestaurant;

public record DeleteRestaurantCommand(int Id) : IRequest<bool>;
