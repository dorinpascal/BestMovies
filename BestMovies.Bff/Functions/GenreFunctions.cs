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
    
    [FunctionName(nameof(GetMoviesByGenre))]
    [OpenApiOperation(operationId: nameof(GetMoviesByGenre), tags: new[] { Tag })]
    [OpenApiParameter(name: "genre", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The the **genre** to list the movies for")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<SearchMovieDto>), Description = "Returns popular movies in the provided genre.")]
    public async Task<IActionResult> GetMoviesByGenre(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "genres/{genre}/movies")]
        HttpRequest req,
        string genre,
        ILogger log)
    {

        //capitalize first letter of string to match API input
        genre = $"{genre.FirstOrDefault().ToString().ToUpper()}{genre[1..]}";
        
        var genres = await _tmDbClient.GetMovieGenresAsync();
        
        var searchedGenre = genres.Find(g => g.Name == genre);

        if (searchedGenre is null)
        {
            return new BadRequestObjectResult("There is no genre with this name");
        }

        var searchedMovies = await _tmDbClient.DiscoverMoviesAsync().IncludeWithAllOfGenre(new[]{searchedGenre}).Query();
        var moviesDtos = searchedMovies.Results.Select(m => m.ToDto(genres));

        return new OkObjectResult(moviesDtos);
    }
}