using System.Buffers.Text;

namespace SongService.Controller;

public class SongRequest
{
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? ImageLink { get; set; }
    public string? AudioLink { get; set; }
}