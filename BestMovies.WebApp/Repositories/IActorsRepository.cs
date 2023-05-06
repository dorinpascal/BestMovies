using BestMovies.Shared.Dtos.Actor;

namespace BestMovies.WebApp.Repositories;

public interface IActorsRepository
{
    Task<ActorDetailsDto?> GetActorDetails(int id);
}