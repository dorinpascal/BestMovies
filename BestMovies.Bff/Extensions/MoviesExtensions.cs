using System;
using BestMovies.Shared.Dtos.Actor;
using BestMovies.Shared.Dtos.Movies;
using System.Collections.Generic;
using System.Linq;
using BestMovies.Shared.Dtos.Director;
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
    
    public static MovieDetailsDto ToDto(this Movie movie, IEnumerable<Cast> actors, Crew? director) =>
        new(
           Id: movie.Id,
           Title: movie.Title,
           Description: movie.Overview,
           OriginalLanguage: movie.OriginalLanguage,
           ReleaseDate: movie.ReleaseDate is null ? null : DateOnly.FromDateTime(movie.ReleaseDate.Value),
           VoteAverage: (decimal)movie.VoteAverage,
           Genres: movie.Genres.Select(g => g.Name).ToList(),
           Actors: actors.Select(a => new ActorDto(a.Id, a.Name, a.Character)),
           Director: director is not null ? new DirectorDto(director.Id, director.Name) : null
            );
    
    public static SearchMovieDto ToSearchDto(this Movie movie) =>
        new(
            Id: movie.Id,
            Title: movie.Title,
            Genres: movie.Genres.Select(g => g.Name).ToList()
        );
}