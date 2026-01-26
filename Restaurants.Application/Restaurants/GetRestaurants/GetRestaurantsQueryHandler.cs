using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Application.Restaurants.GetRestaurants;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.GetAllRestaurants;

public class GetRestaurantsQueryHandler(ILogger<GetRestaurantsQueryHandler> logger,
                                           IMapper mapper,
                                           IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    public async Task<IEnumerable<RestaurantDto>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all restaurants");

        try
        {
            var restaurants = await restaurantsRepository.GetAllAsync();

            var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            return restaurantsDto;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
