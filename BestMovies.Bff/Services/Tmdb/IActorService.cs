using System.Threading.Tasks;
using BestMovies.Shared.Dtos.Person;

namespace BestMovies.Bff.Services.Tmdb;

public interface IActorService
{
    Task<PersonDetailsDto> GetActorDetails(int id);
    Task<byte[]> GetImageBytes(string size, int id);
}