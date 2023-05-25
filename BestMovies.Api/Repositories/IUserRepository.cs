using System.Threading.Tasks;
using BestMovies.Api.Persistence.Entity;

namespace BestMovies.Api.Repositories;

public interface IUserRepository
{
    Task SaveUser(string userId, string email);
    Task<User> GetUser(string identifier);
}