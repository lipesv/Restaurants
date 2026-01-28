using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.CreateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
                                            IRestaurantsRepository restaurantsRepository,
                                            IMapper mapper) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with Id: {RestaurantId}", command.Id);

        var restaurantToUpdate = await restaurantsRepository.GetByIdAsync(command.Id);

        if (restaurantToUpdate is null)
        {
            logger.LogWarning("Restaurant with Id: {RestaurantId} not found", command.Id);
            return false;
        }

        mapper.Map(command, restaurantToUpdate);

        await restaurantsRepository.SaveChanges();

        logger.LogInformation("Restaurant with Id: {RestaurantId} updated successfully", command.Id);

        return true;
    }
}
