using SongService.Entity;

namespace SongService.Repository;

public class SongRepository(SongContext context) : ISongRepository
{
    public void Delete(Guid id)
    {
        Song? song = context.Songs.Find(id);

        if (song is not null)
        {
            context.Songs.Remove(song);
        }
        context.SaveChanges();
    }

    public Song[] List()
    {
        return [..context.Songs];
    }

    public void Save(Song song)
    {
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
            context.Songs.Add(song);
        }
        context.SaveChanges();
    }

    public Song? Single(Guid id)
    {
        return context.Songs.Find(id);
    }
}
