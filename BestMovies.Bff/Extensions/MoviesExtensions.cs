using BestMovies.Shared.Dtos.Actor;
using BestMovies.Shared.Dtos.Movies;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
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



    public static MovieDetailsDto MovieDetailsToDto(this Movie movie, IEnumerable<Cast> actors) =>
        new(
           Id: movie.Id,
           Title: movie.Title,
           Description: movie.Overview,
           OriginalLanguage: movie.OriginalLanguage,
           ReleaseDate: movie.ReleaseDate,
           VoteAverage: movie.VoteAverage,
           Genres: movie.Genres.Select(g => g.Name).ToList(),
           Actors: actors.Select(a => new ActorDto(a.Id, a.Name, a.Character))
            );
}