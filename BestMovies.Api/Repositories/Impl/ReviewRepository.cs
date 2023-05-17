using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Api.Persistence;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.CustomExceptions;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Repositories.Impl;

public class ReviewRepository : IReviewRepository
{
    private readonly BestMoviesDbContext _dbContext;

    public ReviewRepository(BestMoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateReview(int movieId, string userId, int rating, string? comment)
    {
        var review = new Review(movieId, userId, rating, comment);
        
        var existingReview = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
        if (existingReview is not null)
        {
            throw new DuplicateException($"The user with id '{userId}' has already a review for the movie with id '{movieId}'");
        }
        
        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsForMovie(int movieId)
    {
        return await _dbContext.Reviews
            .Where(review => review.MovieId == movieId)
            .Include(r => r.User)
            .ToListAsync();
    }
}
