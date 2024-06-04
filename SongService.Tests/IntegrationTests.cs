using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using SongService.Entity;
using SongService.Repository;

namespace SongService.Tests;

public class IntegrationTests(WebApplicationFactory<Program> factory)
        : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    [Theory]
    [InlineData("/songs")]
    public async Task List_Songs(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode); // Forbidden because no token is provided
    }
}