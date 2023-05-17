using System.Threading.Tasks;

namespace BestMovies.Api.Repo;

public interface ISavedMoviesRepository
{
    Task SaveMovie(string userId, int movieId, bool isWatched);

    Task UpdateSavedMovie(string userId, int movieId);

}