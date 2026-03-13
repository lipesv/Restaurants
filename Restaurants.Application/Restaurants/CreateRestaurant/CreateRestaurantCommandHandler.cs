using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Context.Interface;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.CreateRestaurant;

public class CreateRestaurantCommandHandler(ILogger<CreateRestaurantCommandHandler> logger,
                                            IUserContext userContext,
                                            IMapper mapper,
                                            IRestaurantsRepository restaurantsRepository) : IRequestHandler<CreateRestaurantCommand, int>
{
    public async Task<int> Handle(CreateRestaurantCommand command, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("{UserEmail} [{UserId}] is creating a new restaurant {@Restaurant}",
                              currentUser!.Email,
                              currentUser.Id,
                              command);

        var restaurant = mapper.Map<Restaurant>(command);
        restaurant.OwnerId = currentUser.Id;

        int id = await restaurantsRepository.CreateAsync(restaurant);

        return id;
    }
}
