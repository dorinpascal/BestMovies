

using BestMovies.Shared.Dtos.Review;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Persistance;

public class BestMoviesDbContext:DbContext
{
    public DbSet<Review> Reviews { get; set; }



}
