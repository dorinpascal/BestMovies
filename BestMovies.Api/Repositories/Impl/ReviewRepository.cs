using System.Threading.Tasks;
using BestMovies.Api.Persistence;
using BestMovies.Api.Persistence.Entity;

namespace BestMovies.Api.Repositories.Impl;

public class ReviewRepository : IReviewRepository
{
    private readonly BestMoviesDbContext _dbContext;

    public ReviewRepository(BestMoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateReview(string userId, int rating, string? comment)
    {
        var review = new Review(userId, rating, comment);
        
        await _dbContext.Reviews.AddAsync(review);
        await _dbContext.SaveChangesAsync();
    }
}
