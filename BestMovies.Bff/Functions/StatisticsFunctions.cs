using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Bff.Helpers;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Shared.Dtos.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace BestMovies.Bff.Functions;

public class StatisticsFunctions
{
    private const string Tag = "Statistics";

    private readonly IStatisticsService _statisticsService;

    public StatisticsFunctions(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
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

            var reviews = await _statisticsService.GetMovieStats(id);
            return new OkObjectResult(reviews);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving stats for the movie with id {MovieId}", id);
            return ActionResultHelpers.ServerErrorResult();
        }
    }

    [FunctionName(nameof(GetTopRatedMovies))]
    [OpenApiOperation(operationId: nameof(GetTopRatedMovies), tags: new[] {Tag})]
    public async Task<IActionResult> GetTopRatedMovies([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/topRated")] HttpRequest req, ILogger log)
    {
        try
        {
            var topRatedMovies = await _statisticsService.GetTopRatedMovies();
            return new OkObjectResult(topRatedMovies);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving top rated movies");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}