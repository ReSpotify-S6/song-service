using SongService.Entity;

namespace SongService.Repository;

public interface ISongRepository
{
    public Song[] List();

    public Song? Single(Guid id);

    public void Save(Song song);

    public void Delete(Guid id);
}