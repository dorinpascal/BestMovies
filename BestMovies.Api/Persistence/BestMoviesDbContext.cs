using System.Threading.Tasks;
using BestMovies.Api.Persistence.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Persistence;

// ReSharper disable UnusedAutoPropertyAccessor.Local
public class BestMoviesDbContext : DbContext
{
    public DbSet<Review> Reviews { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<SavedMovie> SavedMovies { get; set; }

    public BestMoviesDbContext(DbContextOptions<BestMoviesDbContext> options) : base(options)
    { }

    public async Task<SqlConnection> OpenDbConnection()
    {
        var connection = new SqlConnection(Database.GetConnectionString());

        await connection.OpenAsync();
        return connection;
    }
    
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
        
        builder.Entity<SavedMovie>(table =>
        {
            table.HasKey(x => new {x.MovieId, x.UserId});
            table.Property(x => x.IsWatched);
        });
    }
}