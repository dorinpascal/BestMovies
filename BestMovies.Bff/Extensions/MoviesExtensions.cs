using BestMovies.Shared.Dtos.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace BestMovies.Bff.Extensions
{
    public static class MoviesExtensions
    {
        public static SearchMovieDto ToDto(this SearchMovie searchMovie, IEnumerable<Genre> genres) =>
        new(
            Id: searchMovie.Id,
            Title: searchMovie.Title,
            PosterPath: searchMovie.PosterPath,
            Genres: genres.Where(g => searchMovie.GenreIds.Contains(g.Id)).Select(g => g.Name)
        );
    }
}
