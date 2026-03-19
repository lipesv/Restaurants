using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Enums;

namespace Restaurants.Application.Restaurants.GetRestaurants;

public record GetRestaurantsQuery(string? SearchPhrase,
                                  int PageNumber,
                                  int PageSize,
                                  string? SortBy,
                                  SortDirection SortDirection) : IRequest<PageResult<RestaurantDto>>;