using BestMovies.Api.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Persistence;

public class BestMoviesDbContext : DbContext
{
    public DbSet<Review> Reviews { get; set; }

    public BestMoviesDbContext(DbContextOptions<BestMoviesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

    }
}