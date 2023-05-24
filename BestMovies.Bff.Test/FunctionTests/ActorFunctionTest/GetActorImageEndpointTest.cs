using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.ActorFunctionTest;

public class GetActorImageEndpointTest
{
    private readonly IActorService _actorService;
    private readonly MockLogger<ActorFunctions> _logger;
    private readonly DefaultHttpRequest _request;
    private readonly ActorFunctions _sut;

    public GetActorImageEndpointTest()
    {
        _request = new DefaultHttpRequest(new DefaultHttpContext());
        _actorService = Substitute.For<IActorService>();
        _logger = Substitute.For<MockLogger<ActorFunctions>>();
        _sut = new ActorFunctions(_actorService);
    }
    
    [Fact]
    public async Task GetActorImage_TmdbApiNotAvailable_ReturnsStatusCode500()
    {
        //Arrange
        _actorService.GetImageBytes(Arg.Any<string>(),Arg.Any<int>()).Throws<Exception>();
        
        //Act
        var response = await _sut.GetActorImage(_request,1, _logger);
        var result = (ContentResult)response;
        
        //Assert
        Assert.Equal(500, result.StatusCode);
        
    }
    
    [Fact]
    public async Task GetActorImage_NoImageAvailable_ReturnsStatusCode404()
    {
        //Arrange
        _actorService.GetImageBytes(Arg.Any<string>(),Arg.Any<int>()).Throws(new NotFoundException("Not found"));

        //Act
        var response = await _sut.GetActorImage(_request, 1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(404, result.StatusCode);

    }

    [Fact]
    public async Task GetActorImage_ActorImageAvailable_ReturnsFileContentResult()
    {
        //Arrange
        var actorImage = new byte[]{0,1,2,3,4,5};
        _actorService.GetImageBytes(Arg.Any<string>(),Arg.Any<int>()).Returns(actorImage);
        
        //Act
        var response = await _sut.GetActorImage(_request, 1, _logger);
        var result = (FileContentResult)response;
        
        //Assert
        Assert.Equal(actorImage, result.FileContents);
    
    }
}