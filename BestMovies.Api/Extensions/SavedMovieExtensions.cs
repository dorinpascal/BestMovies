using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Api.Extensions;

public static class SavedMovieExtensions
{
    public static SavedMovieDto ToDto(this SavedMovie savedMovie) =>
        new(
            MovieId: savedMovie.MovieId,
            IsWatched: savedMovie.IsWatched
        );
}