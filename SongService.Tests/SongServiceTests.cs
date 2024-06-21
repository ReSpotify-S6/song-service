using Moq;
using SongService.Entity;
using SongService.Repository;

public class SongServiceTests
{
    private readonly Mock<ISongRepository> _mockRepository;
    private readonly Dictionary<string, string> _mockEnvVariables;
    private readonly SongService.Services.SongService _service;
    private readonly string _mockApiGatewayHost = "https://mockapi.com";

    public SongServiceTests()
    {
        _mockRepository = new Mock<ISongRepository>();
        _mockEnvVariables = new Dictionary<string, string>
        {
            { "API_GATEWAY_HOST", _mockApiGatewayHost }
        };


        _service = new SongService.Services.SongService(_mockRepository.Object, _mockEnvVariables);
    }

    [Fact]
    public void List_ShouldReturnAllSongs()
    {
        var songs = new[]
        {
            new Song("Song 1", "Artist 1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3"),
            new Song("Song 2", "Artist 2", $"{_mockApiGatewayHost}/images/img2.jpg", $"{_mockApiGatewayHost}/audio/audio2.mp3")
        };

        _mockRepository.Setup(repo => repo.List()).Returns(songs);

        var result = _service.List();

        Assert.Equal(songs, result);
    }

    [Fact]
    public void Single_ShouldReturnSongById()
    {
        var songId = Guid.NewGuid();
        var song = new Song("Song 1", "Artist 1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3") { Id = songId };

        _mockRepository.Setup(repo => repo.Single(songId)).Returns(song);

        var result = _service.Single(songId);

        Assert.Equal(song, result);
    }

    [Fact]
    public void Save_ShouldReturnValidationResultAndSaveSong_WhenValid()
    {
        var song = new Song("Song 1", "Artist 1", $"{_mockApiGatewayHost}/images/img1.jpg", $"{_mockApiGatewayHost}/audio/audio1.mp3");

        _mockRepository.Setup(repo => repo.Save(song));

        var result = _service.Save(song);

        Assert.True(result.IsValid);
        _mockRepository.Verify(repo => repo.Save(song), Times.Once);
    }

    [Fact]
    public void Save_ShouldReturnValidationResultWithErrors_WhenInvalid()
    {
        var song = new Song("", "", "https://wrongapi.com/images/img1.jpg", "https://wrongapi.com/audio/audio1.mp3");

        var result = _service.Save(song);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
        Assert.Contains(result.Errors, e => e.PropertyName == "Artist");
        Assert.Contains(result.Errors, e => e.PropertyName == "ImageLink");
        Assert.Contains(result.Errors, e => e.PropertyName == "AudioLink");
        _mockRepository.Verify(repo => repo.Save(It.IsAny<Song>()), Times.Never);
    }

    [Fact]
    public void Delete_ShouldRemoveSongById()
    {
        var songId = Guid.NewGuid();

        _service.Delete(songId);

        _mockRepository.Verify(repo => repo.Delete(songId), Times.Once);
    }
}