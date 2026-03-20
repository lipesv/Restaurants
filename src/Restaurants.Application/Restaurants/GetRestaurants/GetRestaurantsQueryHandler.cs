using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Application.Restaurants.GetRestaurants;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.GetAllRestaurants;

public class GetRestaurantsQueryHandler(ILogger<GetRestaurantsQueryHandler> logger,
                                        IMapper mapper,
                                        IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetRestaurantsQuery, PageResult<RestaurantDto>>
{
    public async Task<PageResult<RestaurantDto>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants");

        var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase,
                                                                                        request.PageNumber,
                                                                                        request.PageSize,
                                                                                        request.SortBy,
                                                                                        request.SortDirection);

        var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        
        var result = new PageResult<RestaurantDto>(restaurantsDto.ToList(),
                                                   totalCount,
                                                   request.PageSize,
                                                   request.PageNumber);

        return result;
    }
}
