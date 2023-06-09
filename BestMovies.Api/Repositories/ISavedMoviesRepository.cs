﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BestMovies.Api.Persistence.Entity;

namespace BestMovies.Api.Repositories;

public interface ISavedMoviesRepository
{
    Task SaveMovie(string userId, int movieId, bool isWatched);

    Task UpdateSavedMovie(string userId, int movieId, bool isWatched);

    Task<IEnumerable<SavedMovie>> GetSavedMoviesForUser(string userId, bool? isWatched = null);
    
    Task<SavedMovie?> GetSavedMovieForUser(string userId, int movieId);

    Task DeleteSavedMovie(string userId, int movieId);
}