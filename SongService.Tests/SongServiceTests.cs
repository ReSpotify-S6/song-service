using Microsoft.EntityFrameworkCore;
using SongService.Entity;
using SongService.Repository;
using SongService.Services;

public class SongServiceTests
{
    private static readonly string _mockApiGatewayHost = "https://mockapi.com";

    private static readonly Dictionary<string, string> _mockEnvStore = new()
    {
        { "API_GATEWAY_HOST", _mockApiGatewayHost }
    };

    private static SongContext CreateNewContext() => new(
        new DbContextOptionsBuilder<SongContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options);

    private static ISongService CreateNewService(SongContext context) => new SongService.Services.SongService(context, _mockEnvStore);

    [Fact]
    public void List_ReturnsAllSongs()
    {
        // Arrange
        var context = CreateNewContext();
        var service = CreateNewService(context);
        var song1 = new Song("Title1", "Artist1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3");
        var song2 = new Song("Title2", "Artist2", $"{_mockApiGatewayHost}/images/img2.jpg", $"{_mockApiGatewayHost}/audio/audio2.mp3");
        context.Songs.Add(song1);
        context.Songs.Add(song2);
        context.SaveChanges();

        // Act
        var result = service.List();

        // Assert
        Assert.Equal(2, result.Length);
        Assert.Contains(result, s => s.Title == "Title1");
        Assert.Contains(result, s => s.Title == "Title2");
    }

    [Fact]
    public void Single_ReturnsCorrectSong()
    {
        // Arrange
        var context = CreateNewContext();
        var service = CreateNewService(context);
        var song = new Song("Title1", "Artist1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3");
        context.Songs.Add(song);
        context.SaveChanges();

        // Act
        var result = service.Single(song.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(song.Title, result.Title);
    }

    [Fact]
    public void Save_ValidSong_SavesSuccessfully()
    {
        // Arrange
        var context = CreateNewContext();
        var service = CreateNewService(context);
        var song = new Song("Title1", "Artist1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3");

        // Act
        var validationResult = service.Save(song);
        var savedSong = context.Songs.Find(song.Id);

        // Assert
        Assert.True(validationResult.IsValid);
        Assert.NotNull(savedSong);
        Assert.Equal(song.Title, savedSong.Title);
    }

    [Fact]
    public void Save_InvalidSong_ReturnsValidationErrors()
    {
        // Arrange
        var context = CreateNewContext();
        var service = CreateNewService(context);
        var song = new Song("Title1", "Artist1", "invalidlink.jpg", "invalidlink.mp3");

        // Act
        var validationResult = service.Save(song);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Equal(2, validationResult.Errors.Count);
    }

    [Fact]
    public void Delete_RemovesSongSuccessfully()
    {
        // Arrange
        var context = CreateNewContext();
        var service = CreateNewService(context);
        var song = new Song("Title1", "Artist1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3");
        context.Songs.Add(song);
        context.SaveChanges();

        // Act
        service.Delete(song.Id);
        var deletedSong = context.Songs.Find(song.Id);

        // Assert
        Assert.Null(deletedSong);
    }

    [Fact]
    public void OnDeletedAudio_RemovesSongsWithMatchingAudioLink()
    {
        // Arrange
        var context = CreateNewContext();
        var service = CreateNewService(context);
        var song1 = new Song("Title1", "Artist1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3");
        var song2 = new Song("Title2", "Artist2", $"{_mockApiGatewayHost}/images/img2.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3"); // Same audio link
        var song3 = new Song("Title3", "Artist3", $"{_mockApiGatewayHost}/images/img3.jpg", $"{_mockApiGatewayHost}/audio/audio2.mp3");
        context.Songs.AddRange(song1, song2, song3);
        context.SaveChanges();

        // Act
        service.OnDeletedAudio($"{_mockApiGatewayHost}/audio/audio1.mp3");
        var remainingSongs = context.Songs.ToList();

        // Assert
        Assert.Single(remainingSongs);
        Assert.DoesNotContain(remainingSongs, s => s.AudioLink == $"{_mockApiGatewayHost}/audio/audio1.mp3");
        Assert.Contains(remainingSongs, s => s.AudioLink == $"{_mockApiGatewayHost}/audio/audio2.mp3");
    }

    [Fact]
    public void OnDeletedAudio_RemovesSongsWithMatchingImageLink()
    {
        // Arrange
        var context = CreateNewContext();
        var service = CreateNewService(context);
        var song1 = new Song("Title1", "Artist1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3");
        var song2 = new Song("Title2", "Artist2", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio2.mp3"); // Same audio link
        var song3 = new Song("Title3", "Artist3", $"{_mockApiGatewayHost}/images/img3.jpg", $"{_mockApiGatewayHost}/audio/audio3.mp3");
        context.Songs.AddRange(song1, song2, song3);
        context.SaveChanges();

        // Act
        service.OnDeletedImage($"{_mockApiGatewayHost}/images/img1.jpg");
        var remainingSongs = context.Songs.ToList();

        // Assert
        Assert.Single(remainingSongs);
        Assert.DoesNotContain(remainingSongs, s => s.ImageLink == $"{_mockApiGatewayHost}/images/img1.jpg");
        Assert.Contains(remainingSongs, s => s.ImageLink == $"{_mockApiGatewayHost}/images/img3.jpg");
    }
}
