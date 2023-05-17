using System.Threading.Tasks;

namespace BestMovies.Api.Repo.Impl;

public class SavedMoviesRepository : ISavedMoviesRepository
{
    public Task SaveMovie(string userId, int movieId, bool isWatched)
    {
        throw new System.NotImplementedException();
    }

    public Task UpdateSavedMovie(string userId, int movieId)
    {
        throw new System.NotImplementedException();
    }
}