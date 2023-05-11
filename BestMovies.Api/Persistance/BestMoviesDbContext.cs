using BestMovies.Shared.Dtos.Review;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Persistance;

public class BestMoviesDbContext : DbContext
{
    public DbSet<ReviewDto> Reviews { get; set; }

    public BestMoviesDbContext(DbContextOptions<BestMoviesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}