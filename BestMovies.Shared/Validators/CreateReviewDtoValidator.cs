using BestMovies.Shared.Dtos.Review;
using FluentValidation;

namespace BestMovies.Shared.Validators;

public class CreateReviewDtoValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewDtoValidator() 
    {
        RuleFor(x => x.MovieId).GreaterThanOrEqualTo(0).NotNull();
        RuleFor(x => x.Rating).InclusiveBetween(1, 5).NotNull();
        RuleFor(x => x.Comment);
    }
}