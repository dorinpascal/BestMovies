using BestMovies.Shared.Dtos.Person;

namespace BestMovies.WebApp.Repositories;

public interface IActorsRepository
{
    Task<PersonDetailsDto?> GetActorDetails(int id);
}