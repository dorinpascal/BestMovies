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
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<SearchMovieDto>), Description = "Returns popular movies in the region.")]
    public async Task<IActionResult> GetPopularMovies(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies")] HttpRequest req,
        ILogger log)
    {
        var searchContainer = await _tmDbClient.GetMoviePopularListAsync(language: req.Query["language"], region: req.Query["region"]);
        var genres = await _tmDbClient.GetMovieGenresAsync();
        var moviesDtos = searchContainer.Results.Select(m => m.ToDto(genres));
        return new OkObjectResult(moviesDtos);
    }
    

    [FunctionName(nameof(SearchMovie))]
    [OpenApiOperation(operationId: nameof(SearchMovie), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(SearchParametersDto))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<SearchMovieDto>), Description = "Return movies that match the given params")]
    public async Task<IActionResult> SearchMovie(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "movies/discovery")] HttpRequest req, ILogger log)
    {
        var searchedMovie = JsonConvert.DeserializeObject<SearchParametersDto>(await new StreamReader(req.Body).ReadToEndAsync());
        if (searchedMovie is null)
        {
            return new BadRequestObjectResult("Please provide search params");
        }
        try
        {
            var searchedMovies = await _tmDbClient.SearchMovieAsync(searchedMovie.SearchedByTitle);
            var genres = await _tmDbClient.GetMovieGenresAsync();
            var movies = searchedMovies.Results.Select(m => m.ToDto(genres));

            log.LogInformation("Successfully retrieved list of searched movies");
            return new OkObjectResult(movies);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occurred while processing the request");
            return new ContentResult
            {
                StatusCode = 500,
                Content = ex.Message
            };
        }
    }
    
    [FunctionName(nameof(GetMovieImage))]
    [OpenApiOperation(operationId: nameof(GetMovieImage), tags: new[] { Tag })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The movie id.")]
    [OpenApiParameter(name: "size", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The size for the image.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "image/jpg", bodyType: typeof(byte[]), Description = "Returns movie image.")]
    public async Task<IActionResult> GetMovieImage(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id:int}/image")] HttpRequest req,
        int id,
        ILogger log)
    {
        string size = req.Query["size"];
        size ??= "original";
        
        var config = await _tmDbClient.GetConfigAsync();
        if (!config.Images.BackdropSizes.Contains(size))
        {
            return new BadRequestObjectResult($"Please provide a valid size. Available sizes: {string.Join(",", config.Images.BackdropSizes)}");
        }

        var movieImagePaths = await _tmDbClient.GetMovieImagesAsync(id);

        var bestImage = movieImagePaths?.Backdrops.MaxBy(x => x.VoteAverage);
        if (bestImage is null)
        {
            return new NotFoundObjectResult($"Can not find image for the movie with id {id}");
        }
        
        var imageBytes = await _tmDbClient.GetImageBytesAsync(size, bestImage.FilePath);
        
        return new FileContentResult(imageBytes, "image/jpg");
    }
}