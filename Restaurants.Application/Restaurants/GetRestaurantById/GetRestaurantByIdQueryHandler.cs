using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos.Restaurants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(ILogger<GetRestaurantByIdQueryHandler> logger,
                                           IMapper mapper,
                                           IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching restaurant with ID: {RestaurantId}", request.Id);

        try
        {
            var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
                             ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            var restaurantDto = mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
