using System.Collections.Generic;
using System.Threading.Tasks;
using BestMovies.Api.Persistence.Entity;

namespace BestMovies.Api.Repositories;

public interface ISavedMoviesRepository
{
    Task SaveMovie(string userId, int movieId, bool isWatched);

    Task UpdateSavedMovie(string userId, int movieId, bool isWatched);

    Task<List<SavedMovie>> GetSavedMoviesForUser(string userId);

    Task DeleteSavedMovie(string userId, int movieId);
}