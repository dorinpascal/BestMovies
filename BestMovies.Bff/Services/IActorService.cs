using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Actor;

namespace BestMovies.Bff.Services;

public interface IActorService
{
    Task<ActorDetailsDto> GetActorDetails(int id);
    Task<byte[]> GetImageBytes(string size, int id);
}