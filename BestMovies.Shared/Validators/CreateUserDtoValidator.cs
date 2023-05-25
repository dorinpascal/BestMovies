using BestMovies.Shared.Dtos.User;
using FluentValidation;

namespace BestMovies.Shared.Validators;

public class CreateUserDtoValidator : AbstractValidator<UserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.Email).NotNull().EmailAddress();
    }
}