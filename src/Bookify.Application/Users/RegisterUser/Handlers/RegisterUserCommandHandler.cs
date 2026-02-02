using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging.Commands;
using Bookify.Application.Users.RegisterUser.Commands;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Abstractions.Interfaces;
using Bookify.Domain.Users.Entities;
using Bookify.Domain.Users.Interfaces;
using Bookify.Domain.Users.ValuesObjects;

namespace Bookify.Application.Users.RegisterUser.Handlers;

internal sealed class RegisterUserCommandHandler(
    IAuthenticationService authenticationService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var firstName = new FirstName(request.FirstName);
        var lastName = new LastName(request.LastName);
        var email = new Email(request.Email);
        var user = User.Create(
            firstName,
            lastName,
            email);

        var identityId = await authenticationService.RegisterAsync(
            user,
            request.Password,
            cancellationToken);

        user.SetIdentityId(identityId);

        userRepository.Add(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}