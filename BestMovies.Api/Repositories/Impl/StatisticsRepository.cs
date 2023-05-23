using System.Threading.Tasks;
using BestMovies.Api.Persistence;
using BestMovies.Shared.Dtos.Movies;

namespace BestMovies.Api.Repositories.Impl;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly BestMoviesDbContext _dbContext;

    public StatisticsRepository(BestMoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<MovieStatsDto> GetMovieStats(int movieId)
    {
        await using var connection = await _dbContext.OpenDbConnection();
        await using var command = connection.CreateCommand();

        command.CommandText = @"
SELECT * 
FROM 
(
    SELECT
        COUNT( CASE 
            WHEN IsWatched = 1 
                THEN IsWatched
            END) AS [Watched],
        COUNT( CASE
            WHEN IsWatched = 0 
                THEN IsWatched
            END) AS WantToWatch
    FROM [SavedMovies]
    WHERE [MovieId] = @MovieId
) savedMovieStats,
(
    SELECT 
        ROUND(ISNULL(AVG(CAST(Rating AS DECIMAL)), 0), 2) as AvgRating,
        COUNT(CASE
            WHEN COMMENT IS NOT NULL
                THEN COMMENT
            END) AS ReviewsCount
    FROM [Reviews]
    WHERE [MovieId] = @MovieId
) reviewStats
";

        command.Parameters.AddWithValue("@MovieId", movieId);

        await using var reader = await command.ExecuteReaderAsync();
        await reader.ReadAsync();

        var avgRating = (decimal) reader["AvgRating"];
        var reviewsCount = (int) reader["ReviewsCount"];
        var watched = (int) reader["Watched"];
        var wantToWatch = (int) reader["WantToWatch"];
        
        return new MovieStatsDto(avgRating, reviewsCount, watched, wantToWatch);
    }
}