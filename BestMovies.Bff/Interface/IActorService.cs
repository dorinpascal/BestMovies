using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Actor;

namespace BestMovies.Bff.Interface;

public interface IActorService
{
    Task<ActorDetailsDto> GetActorDetails(int id);
}