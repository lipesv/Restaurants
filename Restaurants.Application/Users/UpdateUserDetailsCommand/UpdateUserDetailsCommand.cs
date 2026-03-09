using MediatR;

namespace Restaurants.Application.Users.UpdateUserDetailsCommand;

public record UpdateUserDetailsCommand(DateOnly? DateOfBirth,
                                       string? Nationality) : IRequest;