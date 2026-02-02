using Bookify.Application.Users.RegisterUser.Commands;
using FluentValidation;

namespace Bookify.Application.Users.RegisterUser.Validations;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty();

        RuleFor(c => c.LastName)
            .NotEmpty();

        RuleFor(c => c.Email)
            .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(5);
    }
}