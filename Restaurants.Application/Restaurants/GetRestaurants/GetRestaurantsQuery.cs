using MediatR;
using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Restaurants.GetRestaurants;

public class GetRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>
{
}
