namespace Restaurants.Application.Users.Context.Interface;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
