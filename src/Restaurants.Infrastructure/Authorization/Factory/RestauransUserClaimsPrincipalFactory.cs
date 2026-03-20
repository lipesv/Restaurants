using System.Security.Claims;
using Microsoft.Extensions.Options;

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
