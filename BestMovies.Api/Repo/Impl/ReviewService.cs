using BestMovies.Api.Persistence;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Shared.Dtos.Review;
using System;
using System.Threading.Tasks;

namespace BestMovies.Api.Repo.Impl;

public class ReviewService : IReviewService
{
    private readonly BestMoviesDbContext _dbContext;

    public ReviewService(BestMoviesDbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }

    public async Task CreateReview(string UserId,ReviewDto reviewDto)
    {
        var review = new Review
            {
            UserId=UserId,
            Rating=reviewDto.Rating,
            Comment=reviewDto.Comment,
        };
       
        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();
        
    }
}
