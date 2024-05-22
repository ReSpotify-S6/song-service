using System.ComponentModel.DataAnnotations.Schema;

namespace SongService.Entity;

public class Song(string title, string artist, string imageLink, string audioLink)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = title;
    public string Artist { get; set; } = artist;
    public string ImageLink { get; set; } = imageLink;
    public string AudioLink { get; set; } = audioLink;
}