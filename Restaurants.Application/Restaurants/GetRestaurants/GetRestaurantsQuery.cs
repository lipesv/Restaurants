using MediatR;
using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Restaurants.GetRestaurants;

public record GetRestaurantsQuery(string? SearchPhrase) : IRequest<IEnumerable<RestaurantDto>>;