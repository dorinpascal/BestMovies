using BestMovies.Api.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Persistence;

// ReSharper disable UnusedAutoPropertyAccessor.Local
public class BestMoviesDbContext : DbContext
{
    public DbSet<Review> Reviews { get; private set; }
    public DbSet<User> Users { get; private set; }

    public BestMoviesDbContext(DbContextOptions<BestMoviesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}