using BestMovies.Shared.Dtos.Movies;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace BestMovies.Bff.Extensions;

public static class MoviesExtensions
{
    public static SearchMovieDto ToDto(this SearchMovie searchMovie, IEnumerable<Genre> genres) =>
        new(
            Id: searchMovie.Id,
            Title: searchMovie.Title,
            Genres: searchMovie.GenreIds.Select(id => genres.First(g => g.Id == id).Name)
        );
}