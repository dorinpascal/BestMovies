using BestMovies.Shared.Dtos.Movies;
using FluentValidation;

namespace BestMovies.Shared.Validators;

public class SavedMovieDtoValidator : AbstractValidator<SavedMovieDto>
{
    public SavedMovieDtoValidator()
    {
        RuleFor(x => x.MovieId).GreaterThanOrEqualTo(0).NotNull();
        RuleFor(x => x.IsWatched);
    }
}