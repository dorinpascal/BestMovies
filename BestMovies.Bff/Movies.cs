using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using TMDbLib.Client;
using BestMovies.Shared;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BestMovies.Bff.ResponseModel;

namespace BestMovies.Bff
{
    public class Movies
    {
        private readonly ILogger<Movies> _logger;
        private readonly TMDbClient _tmDbClient;

        public Movies(ILogger<Movies> log, TMDbClient tmDbClient)
        {
            _logger = log;
            _tmDbClient = tmDbClient;
        }

        [FunctionName("GetMovies")]
        [OpenApiOperation(operationId: "Run", tags: new[] { "Movies" })]
        [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<IActionResult> GetMovies(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            Object obj = await _tmDbClient.GetMovieAsync(47964);
            return new OkObjectResult(obj);
        }



        [FunctionName("SearchMovie")]
        [OpenApiOperation(operationId: "SearchMovie", tags: new[] { "Movies" })]
        [OpenApiRequestBody("application/json",typeof(SearchedMovie))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(SearchedMovie), Description = "The OK response")]
        public async Task<IActionResult> SearchMovie(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "post", Route = null)] HttpRequest req)
        {
            SearchedMovie searchedMovie = JsonConvert.DeserializeObject<SearchedMovie>(await new StreamReader(req.Body).ReadToEndAsync());
            try
            {
                List<MoviesDto> movies = (await _tmDbClient.SearchMovieAsync(searchedMovie.searchedByTitle))
                    .Results
                    .Select(movie => new MoviesDto
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        Image = movie.PosterPath,
                        Genres = movie.GenreIds?.ToList() ?? new List<int>()
                    })
                    .ToList();
                _logger.LogInformation("Successfully retrieved list of searched movies.");
                return new OkObjectResult(movies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing the request.");
                return new BadRequestObjectResult(ex.Message);
            }  
        }
    }
}

