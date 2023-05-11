using BestMovies.Api.Persistance;
using BestMovies.Shared.Dtos.Review;
using System;
using System.Threading.Tasks;

namespace BestMovies.Api.Services.Impl;

public class ReviewService : IReviewService
{
    private readonly BestMoviesDbContext _dbContext;

    public ReviewService(BestMoviesDbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }

    public async Task CreateReview(ReviewDto review)
    {
        await _dbContext.Reviews.AddAsync(review);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex) {
            throw new Exception(ex.Message);
        }
        
    }
}
