using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Context.Interface;
using Restaurants.Domain.Entities.Identity;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.UpdateUserDetails;


public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommand> logger,
                                             IUserContext userContext,
                                             IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Updating user: {UserId} with {@Command}",
                              user!.Id,
                              request);

        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken);

        if (dbUser is null)
            throw new NotFoundException(nameof(User), user!.Id);

        dbUser.Nationality = request.Nationality;
        dbUser.DateOfBirth = request.DateOfBirth;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}