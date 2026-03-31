using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Restaurants.Application.Users.Context;
using Restaurants.Application.Users.Context.Interface;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Policies.Requirements;
using Restaurants.Infrastructure.Authorization.Policies.Requirements.Handlers;
using Xunit;

namespace Restaurants.Infrastructure.Tests.Authorization.Policies.Requirements;

public class MinimumRestaurantsRequirementTests
{
    [Fact]
    public async Task HandleRequirementAsync_WhenUserHasSufficientRestaurants_ShouldSucceed()
    {
        // arrange

        var logger = NullLogger<MinimumRestaurantsRequirementHandler>.Instance;

        var currentUser = new CurrentUser("1", "test@example.com", [], null, null);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = currentUser.Id },
            new() { OwnerId = "2" }
        };

        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(repo => repo.GetByOwnerIdAsync(currentUser.Id))
                                .ReturnsAsync(restaurants);


        var requirement = new MinimumRestaurantsRequirement(2);

        var handler = new MinimumRestaurantsRequirementHandler(logger,
                                                               restaurantRepositoryMock.Object,
                                                               userContextMock.Object);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act
        await handler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_WhenUserHasInsufficientRestaurants_ShouldFail()
    {
        // arrange

        var logger = NullLogger<MinimumRestaurantsRequirementHandler>.Instance;

        var currentUser = new CurrentUser("1", "test@example.com", [], null, null);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new() { OwnerId = currentUser.Id }
        };

        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        restaurantRepositoryMock.Setup(repo => repo.GetByOwnerIdAsync(currentUser.Id))
                                .ReturnsAsync(restaurants);


        var requirement = new MinimumRestaurantsRequirement(2);

        var handler = new MinimumRestaurantsRequirementHandler(logger,
                                                               restaurantRepositoryMock.Object,
                                                               userContextMock.Object);

        var context = new AuthorizationHandlerContext([requirement], null, null);

        // act
        await handler.HandleAsync(context);

        // assert
        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();
    }

}