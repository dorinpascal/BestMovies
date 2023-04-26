using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using TMDbLib.Client;

namespace BestMovies.Bff;

public class Movies
{
    private readonly TMDbClient _tmDbClient;

    public Movies(TMDbClient tmDbClient)
    {
        _tmDbClient = tmDbClient;
    }
    
    [FunctionName("movies")]
    public async Task<IActionResult> GetMovies(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
        ILogger log)
    {
        Object obj=  await _tmDbClient.GetMovieAsync(47964);
        return new OkObjectResult(obj);
    }
}