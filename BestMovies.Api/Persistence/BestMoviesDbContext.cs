using BestMovies.Api.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Persistence;

// ReSharper disable UnusedAutoPropertyAccessor.Local
public class BestMoviesDbContext : DbContext
{
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }
    
    public DbSet<SavedMovies> SavedMovies { get; set; }

    public BestMoviesDbContext(DbContextOptions<BestMoviesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>(table =>
        {
            table.HasKey(x => x.Id);
            table.Property(x => x.Email);
        });

        builder.Entity<Review>(table =>
        {
            table.HasKey(x => new {x.MovieId, x.UserId});
            table.HasOne(x => x.User);
            table.Property(x => x.Rating);
            table.Property(x => x.Comment).IsRequired(false);
        });
        
        builder.Entity<SavedMovies>(table =>
        {
            table.HasKey(x => new {x.MovieId, x.UserId});
            table.Property(x => x.IsWatched);
        });
    }
}