using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Restaurants.Application.Context.Interface;

namespace Restaurants.Application.Context;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public CurrentUser? GetCurrentUser()
    {
        var userContext = (httpContextAccessor?.HttpContext?.User)
                          ?? throw new InvalidOperationException("User context is not present.");

        if (userContext.Identity == null || !userContext.Identity.IsAuthenticated)
            return null;

        var userId = userContext.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = userContext.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
        var roles = userContext.FindAll(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
        
        var nationality = userContext.FindFirst(c => c.Type == "Nationality")?.Value;
        
        var dateOfBirthString = userContext.FindFirst(c => c.Type == "DateOfBirth")?.Value;
        var dateOfBirth = dateOfBirthString == null ? (DateOnly?)null : DateOnly.ParseExact(dateOfBirthString,
                                                                                            "yyyy-MM-dd",
                                                                                            CultureInfo.InvariantCulture);


        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
            return null;

        return new CurrentUser(userId,
                               email,
                               roles,
                               nationality,
                               dateOfBirth);
    }
}