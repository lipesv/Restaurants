using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurants.Infrastructure.Authorization.Policies;

namespace Restaurants.API.Tests;

public class TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                             ILoggerFactory logger,
                             UrlEncoder encoder) : AuthenticationHandler<AuthenticationSchemeOptions>(options,
                                                                                                      logger,
                                                                                                      encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "1"),
            new(ClaimTypes.Role, "Owner")
        };

        if (Request.Headers.TryGetValue("X-Test-Nationality", out var nationality))
        {
            claims.Add(new Claim(AppClaimTypes.Nationality, nationality!));
        }

        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Test");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}