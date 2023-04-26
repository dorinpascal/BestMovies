using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Bff.Extensions;
using BestMovies.Shared.Dtos.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TMDbLib.Client;

namespace BestMovies.Bff.Functions;

public class Movies
{
    private readonly TMDbClient _tmDbClient;

    public Movies(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }
    
    [FunctionName(nameof(GetPopularMovies))]
    [OpenApiOperation(operationId: nameof(GetPopularMovies), tags: new[] { nameof(Movies) })]
    [OpenApiParameter(name: "language", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The preferred **language** for the movies")]
    [OpenApiParameter(name: "region", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The preferred **region** for movies recommendation")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(IEnumerable<SearchMovieDto>), Description = "Returns popular movies in the region.")]
    public async Task<IActionResult> GetPopularMovies(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies")] HttpRequest req,
        ILogger log)
    {
        var searchContainer = await _tmDbClient.GetMoviePopularListAsync(language: req.Query["language"], region: req.Query["region"]);

        var genres = await _tmDbClient.GetMovieGenresAsync();
        var moviesDtos = searchContainer.Results.Select(m => m.ToDto(genres));
        return new OkObjectResult(moviesDtos);
    }
}