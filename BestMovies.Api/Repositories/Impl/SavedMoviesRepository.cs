﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Api.Persistence;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Repositories.Impl;

public class SavedMoviesRepository : ISavedMoviesRepository
{
    
    private readonly BestMoviesDbContext _dbContext;

    public SavedMoviesRepository(BestMoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveMovie(string userId, int movieId, bool isWatched)
    {
        var existing = await _dbContext.SavedMovies
            .FirstOrDefaultAsync(sm => sm.UserId == userId && sm.MovieId == movieId);

        if (existing is not null)
        {
            throw new DuplicateException($"A movie with id {movieId} is already in {userId} user's saved list");
        }
        
        var savedMovie = new SavedMovie(userId, movieId, isWatched);
        await _dbContext.SavedMovies.AddAsync(savedMovie);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateSavedMovie(string userId, int movieId, bool isWatched)
    {
        var existing = await _dbContext.SavedMovies
            .FirstOrDefaultAsync(sm => sm.UserId == userId && sm.MovieId == movieId);

        if (existing is null)
        {
            throw new NotFoundException($"A movie with id {movieId} is not found in {userId} user's saved list");
        }

        existing.IsWatched = isWatched;
        _dbContext.SavedMovies.Update(existing);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<SavedMovie>> GetSavedMoviesForUser(string userId, bool? isWatched)
    {
        var movieList = await _dbContext.SavedMovies.Where(sm => sm.UserId == userId).ToListAsync();

        return isWatched != null ? movieList.Where(m=> m.IsWatched == isWatched).ToList() : movieList;
    }

    public async Task<SavedMovie?> GetSavedMovieForUser(string userId, int movieId)
    {
        var savedMovie = await _dbContext.SavedMovies
            .FirstOrDefaultAsync(sm => sm.UserId == userId && sm.MovieId == movieId);
        return savedMovie;
    }

    public async Task DeleteSavedMovie(string userId, int movieId)
    {
        var movieToDelete = await _dbContext.SavedMovies
            .FirstOrDefaultAsync(sm => sm.UserId == userId && sm.MovieId == movieId);
        
        if (movieToDelete is not null)
        {
            _dbContext.SavedMovies.Remove(movieToDelete);
            await _dbContext.SaveChangesAsync();
        }
        
    }
}