namespace BestMovies.Shared.Dtos.Actor;

public record ActorDetailsDto(int Id, string Name, string Biography, DateTime? Birthday, IEnumerable<string>? StarredInMovies);