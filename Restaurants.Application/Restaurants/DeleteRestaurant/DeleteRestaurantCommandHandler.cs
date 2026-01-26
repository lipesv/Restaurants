using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
                                            IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with Id: {RestaurantId}", request.Id);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurant is null)
        {
            logger.LogWarning("Restaurant with Id: {RestaurantId} not found", request.Id);
            return false;
        }

        await restaurantsRepository.DeleteAsync(restaurant);

        return true;
    }
}
