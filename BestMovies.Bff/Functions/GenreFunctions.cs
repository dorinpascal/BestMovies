using System;
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


public class GenreFunctions
{
    private const string Tag = "Genre";
    
    private readonly TMDbClient _tmDbClient;

    public GenreFunctions(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }
    
    [FunctionName(nameof(GetGenreNames))]
    [OpenApiOperation(operationId: nameof(GetGenreNames), tags: new[] { Tag })]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<string>), Description = "Returns the names of all the available genres.")]
    public async Task<IActionResult> GetGenreNames(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "genres")]
        HttpRequest req,
        ILogger log)
    {
        var genres = await _tmDbClient.GetMovieGenresAsync();
        var genreNames = genres.Select(g => g.Name);
        return new OkObjectResult(genreNames);
    }
    
}