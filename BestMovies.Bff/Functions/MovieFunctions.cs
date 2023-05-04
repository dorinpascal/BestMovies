using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Bff.Interface;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Bff.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BestMovies.Bff.Functions;

public class MovieFunctions
{
    private const string Tag = "Movies";
    
    private readonly IMovieService _movieService;

    public MovieFunctions(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [FunctionName(nameof(GetPopularMovies))]
    [OpenApiOperation(operationId: nameof(GetPopularMovies), tags: new[] { Tag })]
    [OpenApiParameter(name: "language", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The preferred **language** for the movies")]
    [OpenApiParameter(name: "region", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The preferred **region** for movies recommendation")]
    [OpenApiParameter(name: "genre", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The **genre** to list the movies for")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<SearchMovieDto>), Description = "Returns popular movies in the region.")]
    public async Task<IActionResult> GetPopularMovies(
         [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies")]
        HttpRequest req,
         ILogger log)
    {
        try
        {
            var region = req.Query["region"];
            var language = req.Query["language"];
            var genre = req.Query["genre"];
            var moviesDtos = await _movieService.GetPopularMovies(genre, region: region, language: language);
            return new OkObjectResult(moviesDtos);
        }
        catch (NotFoundException ex)
        {
            return ExceptionHelpers.CreateContentResult(404, ex.Message);
        }
        catch (Exception ex)
        {
            return ExceptionHelpers.CreateContentResult(500, ex.Message);
        }

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
            log.LogInformation("Search parameters were not provided");
            return new BadRequestObjectResult("Please provide search params");
        }
        try
        {
            var movies = await _movieService.SearchMovie(searchedMovie.SearchedByTitle);
            log.LogInformation("Successfully retrieved list of searched movies");
            return new OkObjectResult(movies);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occurred while processing the request");
            return ExceptionHelpers.CreateContentResult(500, ex.Message);
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
        try
        {
            string size = req.Query["size"];
            size ??= "original";
            var imageBytes = await _movieService.GetImageBytes(size, id);
            return new FileContentResult(imageBytes, "image/jpg");
        }
        catch(NotFoundException ex)
        {
            return ExceptionHelpers.CreateContentResult(404, ex.Message);
        }
        catch (Exception ex)
        {
            return ExceptionHelpers.CreateContentResult(500, ex.Message);
        }
    }


    [FunctionName(nameof(GetMovieDetails))]
    [OpenApiOperation(operationId: nameof(GetMovieDetails), tags: new[] { Tag })]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The movie id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(MovieDetailsDto), Description = "Returns movie details/informations")]
    public async Task<IActionResult> GetMovieDetails(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "movies/{id:int}")] HttpRequest req,
        int id,
        ILogger log)
    {
        try
        {
            if (id <= 0) throw new ArgumentException("Invalid value for the id. The value must be greater than 0");
            var movieDetails = await _movieService.GetMovieDetails(id);
            return new OkObjectResult(movieDetails);
        }
        catch(ArgumentException ex )
        {
            log.LogInformation("Invalid value for the id. The value must be greater than 0");
            return ExceptionHelpers.CreateContentResult(400, ex.Message);
        }

        catch (NotFoundException ex)
        {
            log.LogInformation(ex.Message);
            return ExceptionHelpers.CreateContentResult(404, ex.Message);
        }
        catch (Exception ex)
        {
            log.LogInformation(ex.Message);
            return ExceptionHelpers.CreateContentResult(500, ex.Message);
        }
    }
}
