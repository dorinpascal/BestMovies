using BestMovies.Shared.Dtos.User;
using FluentValidation;

namespace BestMovies.Shared.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(x => x.Id).NotNull().NotEmpty();
        RuleFor(x => x.Email).NotNull().EmailAddress();
    }
}