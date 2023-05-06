using BestMovies.Bff.Functions;
using BestMovies.Bff.Services;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Actor;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.ActorFunctionTest;

public class GetActorDetailsEndpointTest
{
    private readonly IActorService _actorService;
    private readonly MockLogger<ActorFunctions> _logger;
    private readonly DefaultHttpRequest _request;
    private readonly ActorFunctions _sut;

    public GetActorDetailsEndpointTest()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _actorService = Substitute.For<IActorService>();
        _logger = Substitute.For<MockLogger<ActorFunctions>>();
        _sut = new ActorFunctions(_actorService);
    }
    
    [Fact]
    public async Task GetActorDetails_TmdbApiNotAvailable_ReturnsStatusCode500()
    {
        //Arrange
        _actorService.GetActorDetails(Arg.Any<int>()).Throws<Exception>();
        
        //Act
        var response = await _sut.GetActorDetails(_request,1, _logger);
        var result = (ContentResult)response;
        
        //Assert
        Assert.Equal(500, result.StatusCode);
        
    }
    
    [Fact]
    public async Task GetActorDetails_NoDetailsAvailable_ReturnsStatusCode404()
    {
        //Arrange
        _actorService.GetActorDetails(Arg.Any<int>()).Throws(new NotFoundException("Not found"));

        //Act
        var response = await _sut.GetActorDetails(_request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);

    }
    
    [Fact]
    public async Task GetActorDetails_ActorDetails_ReturnsOkObjectResult()
    {
        //Arrange
        var actorDetailsDto = new ActorDetailsDto(0, "Name", "Biography", DateOnly.MinValue, new []{""});
        _actorService.GetActorDetails(Arg.Any<int>()).Returns(actorDetailsDto);

        //Act
        var response = await _sut.GetActorDetails(_request, 1, _logger);
        var result = (OkObjectResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);

    }
    
    [Fact]
    public async Task GetActorDetails_PassedParameter_IsNotValid()
    {
        //Arrange
        
        //Act
        var response = await _sut.GetActorDetails(_request,0,_logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}