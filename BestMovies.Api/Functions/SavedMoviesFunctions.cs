﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Api.Extensions;
using BestMovies.Api.Helpers;
using BestMovies.Api.Persistence.Entity;
using BestMovies.Api.Repositories;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Movies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BestMovies.Api.Functions;

public class SavedMoviesFunctions
{
    private const string Tag = "SavedMovie";

    private readonly ISavedMoviesRepository _savedMoviesRepository;

    public SavedMoviesFunctions(ISavedMoviesRepository savedMoviesRepository)
    {
        _savedMoviesRepository = savedMoviesRepository;
    }
    
    [FunctionName(nameof(AddSavedMovie))]
    [OpenApiOperation(operationId: nameof(AddSavedMovie), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(SavedMovieDto))]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successfully saved the movie")]
    public async Task<IActionResult> AddSavedMovie(
        [HttpTrigger(AuthorizationLevel.Admin, "post", Route = "users/{userId}/savedMovies")] HttpRequest req, string userId, ILogger log)
    { 
        try
        {
            var savedMovieDto = JsonConvert.DeserializeObject<SavedMovieDto>(await new StreamReader(req.Body).ReadToEndAsync());
            if (string.IsNullOrWhiteSpace(userId) || savedMovieDto is null )
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }
            await _savedMoviesRepository.SaveMovie(userId, savedMovieDto.MovieId, savedMovieDto.IsWatched);
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
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while adding a saved movie");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(UpdateSavedMovie))]
    [OpenApiOperation(operationId: nameof(UpdateSavedMovie), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(SavedMovieDto))]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    public async Task<IActionResult> UpdateSavedMovie(
        [HttpTrigger(AuthorizationLevel.Admin, "patch", Route = "users/{userId}/savedMovies")] HttpRequest req, string userId, ILogger log)
    {
        try
        {
            var savedMovieDto = JsonConvert.DeserializeObject<SavedMovieDto>(await new StreamReader(req.Body).ReadToEndAsync());
            if (string.IsNullOrWhiteSpace(userId) || savedMovieDto is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }

            await _savedMoviesRepository.UpdateSavedMovie(userId, savedMovieDto.MovieId, savedMovieDto.IsWatched);
            return new OkResult();
        }
        catch (NotFoundException ex)
        {
            return ActionResultHelpers.NotFoundResult(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while updating the saved movie");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(DeleteSavedMovie))]
    [OpenApiOperation(operationId: nameof(DeleteSavedMovie), tags: new[] { Tag })]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    [OpenApiParameter(name: "movieId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    public async Task<IActionResult> DeleteSavedMovie(
        [HttpTrigger(AuthorizationLevel.Admin, "delete", Route = "users/{userId}/savedMovies/{movieId}")] HttpRequest req, string userId, int movieId, ILogger log)
    {
        try
        {
            if(movieId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }

            await _savedMoviesRepository.DeleteSavedMovie(userId, movieId);
            return new OkResult();
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while updating the saved movie");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(GetSavedMovies))]
    [OpenApiOperation(operationId: nameof(GetSavedMovies), tags: new[] { Tag })]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    [OpenApiParameter(name: "isWatched", In = ParameterLocation.Query, Required = false, Type = typeof(bool), Description = "Return only watched/unwatched movies.")]
    public async Task<IActionResult> GetSavedMovies(
        [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "users/{userId}/savedMovies")] HttpRequest req, string userId, ILogger log)
    { 
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }
            
            var isWatched = bool.TryParse(req.Query["isWatched"], out var result) ? result : default(bool?);

            var savedMoviesForUser = await _savedMoviesRepository.GetSavedMoviesForUser(userId, isWatched);
            
            var dtos = savedMoviesForUser.Select(sm => sm.ToDto());
            
            return new OkObjectResult(dtos);
        }
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving saved movies for user {UserId}", userId);
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(GetSavedMovie))]
    [OpenApiOperation(operationId: nameof(GetSavedMovie), tags: new[] {Tag})]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    [OpenApiParameter(name: "movieId", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The movie id.")]
    
    public async Task<IActionResult> GetSavedMovie(
        [HttpTrigger(AuthorizationLevel.Admin, "get", Route = "users/{userId}/savedMovies/{movieId:int}")] 
        HttpRequest req, string userId, int movieId, ILogger log)
    {
        try
        {
            if(movieId <= 0 || string.IsNullOrWhiteSpace(userId))
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }
            var savedMovie = await _savedMoviesRepository.GetSavedMovieForUser(userId, movieId);

            if (savedMovie is null)
            {
                return ActionResultHelpers.NotFoundResult($"Saved movie with id {movieId} not found for user {userId}");
            }
    
            return new OkObjectResult(savedMovie.ToDto());
        }
        
        catch (ArgumentException ex)
        {
            return ActionResultHelpers.BadRequestResult(ex.Message);
        }
         
        catch (Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving the movie");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}