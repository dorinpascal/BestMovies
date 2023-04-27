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
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using BestMovies.Bff.Extensions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

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
            
            var searchedMovie = JsonConvert.DeserializeObject<SearchedMovie>(await new StreamReader(req.Body).ReadToEndAsync());
            if(searchedMovie.IsNullOrDefault())
            {
                return new OkObjectResult(null);
            }
            try
            {
                var searchedMovies = await _tmDbClient.SearchMovieAsync(searchedMovie.searchedByTitle);
                var genres = await _tmDbClient.GetMovieGenresAsync();
                var movies = searchedMovies.Results.Select(m => m.ToDto(genres));

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

