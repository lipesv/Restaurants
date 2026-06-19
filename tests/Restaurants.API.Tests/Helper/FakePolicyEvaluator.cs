using Microsoft.AspNetCore.Authorization;

namespace Restaurants.API.Tests.Helper;

/// <summary>
/// A fake policy evaluator for unit testing purposes.
/// </summary>
public class FakePolicyEvaluator : IPolicyEvaluator
{
    /// <summary>
    /// Authenticates the user asynchronously.
    /// </summary>
    /// <param name="policy">The authorization policy.</param>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy,
                                                      HttpContext context)
    {
        var claimsPrincipal = new ClaimsPrincipal();

        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Admin")
            ])
        );

        var ticket = new AuthenticationTicket(claimsPrincipal, "Test");
        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }

    /// <summary>
    /// Authorizes the user asynchronously.
    /// </summary>
    /// <param name="policy">The authorization policy.</param>
    /// <param name="authenticationResult">The result of the authentication process.</param>
    /// <param name="context">The HTTP context.</param>
    /// <param name="resource">The resource being accessed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
                                                          AuthenticateResult authenticationResult, HttpContext context,
                                                          object? resource)
    {
        var result = PolicyAuthorizationResult.Success();
        return Task.FromResult(result);
    }
}
