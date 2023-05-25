using System.Threading.Tasks;
using BestMovies.Shared.Dtos.User;

namespace BestMovies.Bff.Services.BestMoviesApi;

public interface IUserService
{
    Task SaveUser(UserDto user);
    Task<UserDto?> GetUserOrDefault(string identifier);
    Task<UserDto> GetUserOrCreate(UserDto user);
}