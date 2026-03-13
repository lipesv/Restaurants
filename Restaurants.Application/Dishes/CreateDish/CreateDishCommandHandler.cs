using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Authorization.Enums;
using Restaurants.Domain.Authorization.Interfaces;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
namespace Restaurants.Application.Dishes.CreateDish;

public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
                                      IRestaurantsRepository restaurantsRepository,
                                      IDishesRepository dishesRepository,
                                      IMapper mapper,
                                      IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<CreateDishCommand, int>
{
    public async Task<int> Handle(CreateDishCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new dish: {@DishRequest}", command);
        var restaurant = await restaurantsRepository.GetByIdAsync(command.RestaurantId);

        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), command.RestaurantId.ToString());

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Create))
            throw new ForbiddenException();

        var dish = mapper.Map<Dish>(command);

        return await dishesRepository.CreateAsync(dish);
    }
}
