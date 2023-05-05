using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Bff.Interface;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Actor;
using TMDbLib.Client;

namespace BestMovies.Bff.Services;

public class ActorService : IActorService
{
    private readonly TMDbClient _tmDbClient;

    public ActorService(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }

    public async Task<ActorDetailsDto> GetActorDetails(int id)
    {
       var person = await _tmDbClient.GetPersonAsync(id);
       if (person is null)
       {
           throw new NotFoundException($"Cannot find any actor with id '{id}'");
       }
       
       var movieCredits = await _tmDbClient.GetPersonMovieCreditsAsync(id);
       return person.ToDto(movieCredits);
    }
}