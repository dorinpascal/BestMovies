using BestMovies.Shared.Dtos.Actor;

namespace BestMovies.Shared.Dtos.Movies
{
    public record MovieDetailsDto(int Id, string Title, string Description, string OriginalLanguage, DateTime? ReleaseDate, double VoteAverage, IEnumerable<string> Genres, IEnumerable<ActorDto> Actors);

}
