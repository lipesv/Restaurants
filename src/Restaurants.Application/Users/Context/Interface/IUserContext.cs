using Restaurants.Application.Users.Context;

namespace Restaurants.Application.Users.Context.Interface;


public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
