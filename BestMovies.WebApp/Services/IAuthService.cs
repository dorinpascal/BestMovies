using BestMovies.WebApp.Authorization;

namespace BestMovies.WebApp.Services;

public interface IAuthService
{
    Task<UserInformation?> RetrieveUserInformation();
}