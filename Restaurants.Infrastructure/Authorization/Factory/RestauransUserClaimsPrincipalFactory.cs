using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Infrastructure.Auth.Policies;

namespace Restaurants.Infrastructure.Authorization.Factory;

public class RestauransUserClaimsPrincipalFactory(UserManager<User> userManager,
                                                  RoleManager<IdentityRole> roleManager,
                                                  IOptions<IdentityOptions> options) : UserClaimsPrincipalFactory<User, IdentityRole>(userManager,
                                                                                                                                      roleManager,
                                                                                                                                      options)
{
    public override async Task<ClaimsPrincipal> CreateAsync(User user)
    {
        var principal = await GenerateClaimsAsync(user);

        if (!string.IsNullOrEmpty(user.Nationality))
        {
            principal.AddClaim(new Claim(AppClaimTypes.Nationality, user.Nationality));
        }

        if (user.DateOfBirth != null)
        {
            principal.AddClaim(new Claim(AppClaimTypes.DateOfBirth,
                                         user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
        }

        return new ClaimsPrincipal(principal);
    }
}
