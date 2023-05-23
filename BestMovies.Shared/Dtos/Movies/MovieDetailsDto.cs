using BestMovies.Shared.Dtos.Person.Actor;
using BestMovies.Shared.Dtos.Person.Director;

namespace BestMovies.Shared.Dtos.Movies;

public record MovieDetailsDto(int Id, string Title, string Description, string OriginalLanguage, DateOnly? ReleaseDate, decimal VoteAverage, IEnumerable<string> Genres, IEnumerable<ActorDto> Actors, DirectorDto? Director);