using MediatR;

namespace Restaurants.Application.Users.UpdateUserDetails;

public record UpdateUserDetailsCommand(DateOnly? DateOfBirth,
                                       string? Nationality) : IRequest;