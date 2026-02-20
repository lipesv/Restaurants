using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.CreateRestaurant;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
                                            IRestaurantsRepository restaurantsRepository,
                                            IMapper mapper) : IRequestHandler<UpdateRestaurantCommand>
{
    public async Task Handle(UpdateRestaurantCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with Id: {RestaurantId} with {@UpdatedRestaurant}",
                              command.Id,
                              command);

        var restaurantToUpdate = await restaurantsRepository.GetByIdAsync(command.Id);

        if (restaurantToUpdate is null)
        {
            throw new NotFoundException(nameof(Restaurant), command.Id.ToString());
        }

        mapper.Map(command, restaurantToUpdate);

        await restaurantsRepository.SaveChanges();

        logger.LogInformation("Restaurant with Id: {RestaurantId} updated successfully", command.Id);
    }
}
