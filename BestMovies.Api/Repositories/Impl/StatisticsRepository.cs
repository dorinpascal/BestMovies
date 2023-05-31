using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BestMovies.Api.Persistence;
using BestMovies.Shared.Dtos.Movies;
using Microsoft.EntityFrameworkCore;

namespace BestMovies.Api.Repositories.Impl;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly BestMoviesDbContext _dbContext;

    public StatisticsRepository(BestMoviesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<decimal> GetAverageRatingOfMovies(IEnumerable<int> movieIds)
    {
        var movies = await _dbContext.Reviews
            .Where(m => movieIds.Any(id => id == m.MovieId))
            .ToListAsync();
        
        if (!movies.Any())
        {
            return decimal.Zero;
        }

        var average = movies.Average(m => m.Rating);
        return (decimal) Math.Round(average, 2, MidpointRounding.AwayFromZero);
    }
    
    public async Task<IEnumerable<int>> GetTopRatedMovieIds(int count)
    {
        const int minimumReviewCount = 0;
        await using var connection = await _dbContext.OpenDbConnection();
        await using var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT TOP (@NumberOfMovies) [MovieId]
            FROM [Reviews]
            GROUP BY [MovieId]
            HAVING COUNT(*) >= @MinimumReviewCount
            ORDER BY AVG(CAST(Rating AS FLOAT)) DESC;
        ";
        
        command.Parameters.AddWithValue("@NumberOfMovies", count);
        command.Parameters.AddWithValue("@@MinimumReviewCount", minimumReviewCount);
        
        await using var reader = await command.ExecuteReaderAsync();
        
        var movieIdsList = new List<int>();
        
        while (await reader.ReadAsync())
        {
            movieIdsList.Add(reader.GetInt32(0));
        }

        return movieIdsList;
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