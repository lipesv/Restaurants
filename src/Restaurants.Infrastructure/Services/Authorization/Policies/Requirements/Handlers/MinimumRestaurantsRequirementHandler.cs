namespace Restaurants.Infrastructure.Authorization.Policies.Requirements.Handlers;

internal class MinimumRestaurantsRequirementHandler(ILogger<MinimumRestaurantsRequirementHandler> logger,
                                                    IRestaurantsRepository restaurantsRepository,
                                                    IUserContext userContext) : AuthorizationHandler<MinimumRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                         MinimumRestaurantsRequirement requirement)
    {

        var currentUser = userContext.GetCurrentUser();

        if (currentUser == null)
        {
            logger.LogInformation("Authorization failed: user is not authenticated");
            context.Fail();
            return;
        }

        var restaurants = await restaurantsRepository.GetByOwnerIdAsync(currentUser!.Id);

        if (restaurants == null || !restaurants.Any())
        {
            logger.LogInformation("Authorization failed: user does not own any restaurants");
            context.Fail();
            return;
        }

        if (restaurants.Count() < requirement.MinimumRestaurants)
        {
            logger.LogInformation("Authorization failed: user does not have enough restaurants");
            context.Fail();
            return;
        }

        logger.LogInformation("Authorization succeeded for user with ID: {UserId}", currentUser.Id);
        context.Succeed(requirement);

    }
}