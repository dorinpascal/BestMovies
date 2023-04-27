using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;
using TMDbLib.Client;

namespace BestMovies.Bff.Functions;

public class MovieFunctions
{
    private const string Tag = "Movies";
    
    private readonly TMDbClient _tmDbClient;

    public MovieFunctions(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }
    
    [FunctionName(nameof(GetPopularMovies))]
    [OpenApiOperation(operationId: nameof(GetPopularMovies), tags: new[] { Tag })]
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


    [FunctionName("SearchMovie")]
    [OpenApiOperation(operationId: "SearchMovie", tags: new[] { "Movies" })]
    [OpenApiRequestBody("application/json", typeof(SearchParametersDto))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(SearchParametersDto), Description = "The OK response")]
    public async Task<IActionResult> SearchMovie(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req, ILogger log)
    { 

        var searchedMovie = JsonConvert.DeserializeObject<SearchParametersDto>(await new StreamReader(req.Body).ReadToEndAsync());
        if (searchedMovie is null)
        {
            return new BadRequestObjectResult("Wrong body parameter.");
        }
        try
        {
            var searchedMovies = await _tmDbClient.SearchMovieAsync(searchedMovie.searchedByTitle);
            var genres = await _tmDbClient.GetMovieGenresAsync();
            var movies = searchedMovies.Results.Select(m => m.ToDto(genres));

            log.LogInformation("Successfully retrieved list of searched movies.");
            return new OkObjectResult(movies);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occurred while processing the request.");
            return new ObjectResult(new
            {
                StatusCode = 500,
                Value = ex.Message
            });
        }
    }
}