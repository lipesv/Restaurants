namespace Restaurants.Application.Context.Interface;


public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}
