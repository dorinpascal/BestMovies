using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.BestMoviesApi;
using BestMovies.Bff.Test.Helpers;
using BestMovies.Shared.CustomExceptions;
using BestMovies.Shared.Dtos.Review;
using BestMovies.Shared.Dtos.User;
using NSubstitute.ExceptionExtensions;

namespace BestMovies.Bff.Test.FunctionTests.ReviewFunctionTest;

public class AddReviewEndpointTests
{
    private readonly DefaultHttpRequest _request;
    private readonly IReviewService _reviewService;
    private readonly IUserService _userService;
    private readonly MockLogger<ReviewFunctions> _logger;
    private readonly ReviewFunctions _sut;
    
    public AddReviewEndpointTests()
    {
        var reviewDto = new CreateReviewDto(1, 5, "comment");
        _request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reviewDto)))
        };
        _request.Headers.Add("x-ms-client-principal","ewogICJpZGVudGl0eVByb3ZpZGVyIjogImdvb2dsZSIsCiAgInVzZXJJZCI6ICIxIiwKICAidXNlckRldGFpbHMiOiAiPGVtYWlsPkBnbWFpbCIsCiAgInVzZXJSb2xlcyI6IFsiYW5vbnltb3VzIiwgImF1dGhlbnRpY2F0ZWQiXQp9");
        _reviewService = Substitute.For<IReviewService>();
        _userService = Substitute.For<IUserService>();
        _logger = Substitute.For<MockLogger<ReviewFunctions>>();
        _sut = new ReviewFunctions(_reviewService, _userService);
    }
    
    [Fact]
    public async Task AddReviewEndpoint_BestMoviesApi_NotAvailable()
    {
        //Arrange
        _reviewService.AddReview(Arg.Any<UserDto>(),Arg.Any<CreateReviewDto>()).Throws(new Exception());
        
        //Act
        var response = await _sut.AddReview(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(500, result.StatusCode);
    }
    
    [Fact]
    public async Task AddReviewEndpoint_ReviewAlreadyExists_DuplicateException()
    {
        //Arrange
        _reviewService.AddReview(Arg.Any<UserDto>(),Arg.Any<CreateReviewDto>()).Throws(new DuplicateException("Review already exists"));
        
        //Act
        var response = await _sut.AddReview(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(409, result.StatusCode);
    }
    
    [Fact]
    public async Task AddReviewEndpoint_ThrowsArgumentException_BadRequest()
    {
        //Arrange
        _reviewService.AddReview(Arg.Any<UserDto>(),Arg.Any<CreateReviewDto>()).Throws(new ArgumentException());
        
        //Act
        var response = await _sut.AddReview(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task AddReviewEndpoint_UserUnauthorized_UnauthorizedResult()
    {
        //Arrange
        _request.Headers.Remove("x-ms-client-principal");

        //Act
        var response = await _sut.AddReview(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(401, result.StatusCode);
    }
    
    [Fact]
    public async Task AddReviewEndpoint_ReviewAdded_ReturnsSC200()
    {
        //Arrange
        
        //Act
        var response = await _sut.AddReview(_request,1, _logger);
        var result = (StatusCodeResult)response;

        //Assert
        Assert.Equal(200, result.StatusCode);
    }
    
    [Fact]
    public async Task AddReviewEndpoint_ReviewIsNull_ReturnsSC400()
    {
        //Arrange
        _request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(null)));
        
        //Act
        var response = await _sut.AddReview(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
    
    [Fact]
    public async Task AddReviewEndpoint_MovieIdDoesNotMatch_ReturnsSC400()
    {
        //Arrange
        _request.Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new CreateReviewDto(2, 5, "comment"))));
        
        //Act
        var response = await _sut.AddReview(_request,1, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}