using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BestMovies.Api.Persistance.Entity;

public class Review
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
