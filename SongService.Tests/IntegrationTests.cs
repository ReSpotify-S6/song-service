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
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }
}