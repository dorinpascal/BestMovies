using FluentValidation;

namespace BestMovies.Shared.Validators;

public class EmailValidator : AbstractValidator<string>
{
    public EmailValidator()
    {
        RuleFor(email => email).EmailAddress();
    }
}