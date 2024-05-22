using SongService.DependencyInjection;
using SongService.Entity;

namespace SongService.Repository;

[TransientService]
public class SongRepository : ISongRepository
{
    private readonly IConfiguration _configuration;

    public SongRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Delete(Guid id)
    {
        using var context = new SongContext(_configuration);
        Song? song = context.Songs.Find(id);

        if (song is not null)
        {
            context.Songs.Remove(song);
        }
        context.SaveChanges();
    }

    public Song[] List()
    {
        using var context = new SongContext(_configuration);
        return [..context.Songs];
    }

    public void Save(Song song)
    {
        using var context = new SongContext(_configuration);
        Song? dbSong = context.Songs.Find(song.Id);

        if (dbSong is not null) // User wants to update song
        {
            dbSong.Title = song.Title;
            dbSong.Artist = song.Artist;
            dbSong.AudioLink = song.AudioLink;
            dbSong.ImageLink = song.ImageLink;
        }
        else
        {
            var newSong = context.Songs.Add(song);
        }
        context.SaveChanges();
    }

    public Song? Single(Guid id)
    {
        using var context = new SongContext(_configuration);
        return context.Songs.Find(id);
    }
}
