using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Context.Interface;

namespace Restaurants.Infrastructure.Auth.Policies.Requirements.Handlers;

public class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger,
                                          IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                   MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        if (currentUser == null)
        {
            logger.LogInformation("Authorization failed: user is not authenticated");
            context.Fail();
            return Task.CompletedTask;
        }

        logger.LogInformation("User: {Email}, date of birth: {DateOfBirth}",
                              currentUser.Email,
                              currentUser.DateOfBirth);

        if (currentUser.DateOfBirth == null)
        {
            logger.LogInformation("Authorization failed: date of birth not provided");
            context.Fail();
            return Task.CompletedTask;
        }

        var dateOfBirth = currentUser.DateOfBirth.Value;
        var today = DateOnly.FromDateTime(DateTime.Today);

        if (dateOfBirth > today)
        {
            logger.LogInformation("Authorization failed: date of birth is in the future");
            context.Fail();
            return Task.CompletedTask;
        }

        var age = today.Year - dateOfBirth.Year;

        if (age <= requirement.MinimumAge)
        {
            logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}