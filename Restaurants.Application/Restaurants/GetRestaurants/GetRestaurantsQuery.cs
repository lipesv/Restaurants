using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Dtos.Restaurants;

namespace Restaurants.Application.Restaurants.GetRestaurants;

public record GetRestaurantsQuery(string? SearchPhrase,
                                  int PageNumber,
                                  int PageSize) : IRequest<PageResult<RestaurantDto>>;