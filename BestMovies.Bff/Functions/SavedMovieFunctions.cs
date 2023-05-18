using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using BestMovies.Bff.Authorization;
using BestMovies.Bff.Helpers;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using BestMovies.Shared.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BestMovies.Bff.Functions;

public class SavedMovieFunctions
{
    private const string Tag = "Saved Movies"; 
    
    private readonly ISavedMovieService _savedMovieService;

    public SavedMovieFunctions(ISavedMovieService savedMovieService)
    {
        _savedMovieService = savedMovieService;
    }
    
    [FunctionName(nameof(SaveMovie))]
    [OpenApiOperation(operationId: nameof(SaveMovie), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(SavedMovieDto))]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    [OpenApiParameter(name: "x-ms-client-principal", In = ParameterLocation.Header, Required = true, Type = typeof(string), Description = "base64 of ClientPrincipal")]
    public async Task<IActionResult> SaveMovie(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "movies/{id:int}/savedMovies")] HttpRequest req, int id, ILogger log)
    {
        try
        {
            var claims = req.RetrieveClaimsPrincipal();
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (claims.Identity is null || !claims.Identity.IsAuthenticated || string.IsNullOrEmpty(userId))
            {
                return ActionResultHelpers.UnauthorizedResult();
            }

            var savedMovie = JsonConvert.DeserializeObject<SavedMovieDto>(await new StreamReader(req.Body).ReadToEndAsync());
            if (savedMovie is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }

            if (id != savedMovie.MovieId)
            {
                return ActionResultHelpers.BadRequestResult("The movie id from path does not match the one from body");
            }

            var user = new CreateUserDto(
                Id: userId,
                Email: claims.Identity!.Name!
            );
            
            await _savedMovieService.SaveMovie(savedMovie, user);
            
            return new OkResult();
        }
        catch (DuplicateException ex)
        {
            return ActionResultHelpers.Conflict(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while adding a review");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}