using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BestMovies.Api.Persistence.Entity;

public class Review
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
