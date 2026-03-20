using MediatR;
using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Restaurants.GetRestaurantById;

public record GetRestaurantByIdQuery(int Id) : IRequest<RestaurantDto>;

