using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Bff.Interface;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Actor;
using TMDbLib.Client;
using TMDbLib.Objects.People;

namespace BestMovies.Bff.Service;

public class ActorService : IActorService
{
    private readonly TMDbClient _tmDbClient;

    public ActorService(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }

    public async Task<ActorDetailsDto> GetActorDetails(int id)
    {
       var person =  await _tmDbClient.GetPersonAsync(id);
       if (person is null) throw new NotFoundException();
       var movieCredits = await _tmDbClient.GetPersonMovieCreditsAsync(id);
       return person.ToDto(movieCredits);
    }
}