using BestMovies.Api.Persistence;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.Dtos.Review;
using System;
using System.Threading.Tasks;

namespace BestMovies.Api.Repo.Impl;

public class ReviewRepository : IReviewRepository
{
    private readonly BestMoviesDbContext _dbContext;

    public ReviewRepository(BestMoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateReview(string userId,ReviewDto reviewDto)
    {
        var review = new Review
            {
            UserId=userId,
            Rating=reviewDto.Rating,
            Comment=reviewDto.Comment,
        };
       
        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();
        
    }
}
