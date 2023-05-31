using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Api.Helpers;
using BestMovies.Api.Repositories;
using BestMovies.Shared.Dtos.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BestMovies.Api.Functions;

public class StatisticsFunctions
{
    private const string Tag = "Statistics";

    private readonly IStatisticsRepository _statisticsRepository;

    public StatisticsFunctions(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }

    [FunctionName(nameof(GetMovieStats))]
    [OpenApiOperation(operationId: nameof(GetMovieStats), tags: new[] {Tag})]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(MovieStatsDto), Description = "Returns stats for the given movie.")]
    public async Task<IActionResult> GetMovieStats(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id:int}/stats")] HttpRequest req, int id, ILogger log)
    {
        try
        {
            if (id <= 0)
            {
                return ActionResultHelpers.BadRequestResult("Invalid value for the id. The value must be greater than 0");
            }

            var reviews = await _statisticsRepository.GetMovieStats(id);
            return new OkObjectResult(reviews);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving stats for the movie with id {MovieId}", id);
            return ActionResultHelpers.ServerErrorResult();
        }
    }

    [FunctionName(nameof(GetTopRatedMovieIds))]
    [OpenApiOperation(operationId: nameof(GetTopRatedMovieIds), tags: new[] {Tag})]
    public async Task<IActionResult> GetTopRatedMovieIds(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/topRated")] HttpRequest req, ILogger log)
    {
        try
        {
            var topRatedMovieIds = await _statisticsRepository.GetTopRatedMovieIds();
            return new OkObjectResult(topRatedMovieIds);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving top rated movie ids");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(GetAverageRatingOfMovies))]
    [OpenApiOperation(operationId: nameof(GetAverageRatingOfMovies), tags: new[] {Tag})]
    [OpenApiParameter(name: "movieIds", In = ParameterLocation.Query, Required = true, Type = typeof(IEnumerable<int>), Description = "The movie ids.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(decimal), Description = "Returns the average rating for the given movies.")]
    public async Task<IActionResult> GetAverageRatingOfMovies(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/stats")] HttpRequest req, ILogger log)
    {
        try
        {
            string movieIdsString = req.Query["movieIds"];
            var movieIds = movieIdsString.Split(',').Select(int.Parse);

            var averageRating = await _statisticsRepository.GetAverageRatingOfMovies(movieIds);
            return new OkObjectResult(averageRating);
        }
        catch (FormatException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving stats for the movie with id {MovieId}", 1);
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}