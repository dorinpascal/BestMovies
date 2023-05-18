using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using BestMovies.Api.Extensions;
using BestMovies.Api.Helpers;
using BestMovies.Api.Repositories;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace BestMovies.Api.Functions;

public class UsersFunctions
{
    private const string Tag = "User";
    
    private readonly IUserRepository _userRepository;
     
    public UsersFunctions(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [FunctionName(nameof(SaveUser))]
    [OpenApiOperation(operationId: nameof(SaveUser), tags: new[] { Tag })]
    [OpenApiRequestBody("application/json", typeof(CreateUserDto))]
    [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK, Description = "Successfully saved the user")]
    public async Task<IActionResult> SaveUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req, ILogger log)
    {
        try
        {
            var userDto = JsonConvert.DeserializeObject<CreateUserDto>(await new StreamReader(req.Body).ReadToEndAsync());
            if (userDto is null)
            {
                return ActionResultHelpers.BadRequestResult("Invalid parameters.");
            }

            await _userRepository.SaveUser(userDto.Id, userDto.Email);
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
            log.LogError(ex, "Error occured while saving a user");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
    
    [FunctionName(nameof(GetUser))]
    [OpenApiOperation(operationId: nameof(GetUser), tags: new[] { Tag })]
    [OpenApiParameter(name: "userId", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The user id.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(UserDto), Description = "Retrieve user based on the id")]
    public async Task<IActionResult> GetUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId}")] HttpRequest req, string userId, ILogger log)
    { 
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ActionResultHelpers.BadRequestResult("Invalid value for the userId.");
        }
        
        try
        {
            var user = await _userRepository.GetUser(userId);

            return new OkObjectResult(user.ToDto());
        }
        catch (NotFoundException ex)
        {
            return ActionResultHelpers.NotFoundResult(ex.Message);
        }
        catch(Exception ex)
        {
            log.LogError(ex, "Error occured while retrieving a user");
            return ActionResultHelpers.ServerErrorResult();
        }
    }
}