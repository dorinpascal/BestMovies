using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api;

public class BestMoviesDbContext : DbContext
{
    public BestMoviesDbContext(DbContextOptions<BestMoviesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
    }
}