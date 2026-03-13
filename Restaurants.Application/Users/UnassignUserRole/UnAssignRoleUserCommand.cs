using MediatR;
namespace Restaurants.Application.Users.UnassignUserRole;

public record UnassignUserRoleCommand(string UserEmail, string RoleName) : IRequest;