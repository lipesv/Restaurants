using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Policies.Requirements;

public record MinimumRestaurantsRequirement(int MinimumRestaurants) : IAuthorizationRequirement;