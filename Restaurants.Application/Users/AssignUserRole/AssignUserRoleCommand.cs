using MediatR;

namespace Restaurants.Application.Users.AssignUserRole;

public record AssignUserRoleCommand(string UserEmail, string RoleName) : IRequest;