using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.DeleteDishesByRestaurant;

public class DeleteDishesByRestaurantCommandHandler(ILogger<DeleteDishesByRestaurantCommandHandler> logger,
                                                    IRestaurantsRepository restaurantsRepository,
                                                    IDishesRepository dishesRepository) : IRequestHandler<DeleteDishesByRestaurantCommand>
{
    public async Task Handle(DeleteDishesByRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing all dishes from restaurant with Id: {RestaurantId}", request.restaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.restaurantId);

        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.restaurantId.ToString());

        await dishesRepository.DeleteAsync(restaurant.Dishes);
    }
}