using BestMovies.Shared.Dtos.Actor;

namespace BestMovies.Shared.Dtos.Movies;

public record MovieDetailsDto(int Id, string Title, string Description, string OriginalLanguage, DateOnly? ReleaseDate, decimal VoteAverage, IEnumerable<string> Genres, IEnumerable<ActorDto> Actors);