﻿using BestMovies.Bff.Functions;
using BestMovies.Bff.Services.Tmdb;
using BestMovies.Bff.Test.Helpers;

namespace BestMovies.Bff.Test.FunctionTests.MoviesFunctionTest.SearchMovieEndpointTest;

// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
public class BodyParametersTest
{
    private readonly IMovieService _movieService;
    private readonly MovieFunctions _sut;
    private readonly MockLogger<MovieFunctions> _logger;
    public BodyParametersTest()
    {
        _movieService = Substitute.For<IMovieService>();
        _logger = Substitute.For<MockLogger<MovieFunctions>>();
        _sut = new MovieFunctions(_movieService);
    }

    [Fact]
    public async Task SearchMovieEndpoint_BodyParameters_IsNotProvided()
    {
        //Arrange
        var request = new DefaultHttpRequest(new DefaultHttpContext())
        {
            Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject("")))
        };

        //Act
        var response = await _sut.SearchMovie(request, _logger);
        var result = (ContentResult)response;

        //Assert
        Assert.Equal(400, result.StatusCode);
    }
}