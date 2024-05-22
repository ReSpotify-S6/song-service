using SongService.DependencyInjection;
using SongService.Entity;
using SongService.Repository;

namespace SongService.Services;

[TransientService]
public class SongService(ISongRepository repository) : ISongService
{
    private readonly ISongRepository _repository = repository;

    public Song[] List()
    {
        return _repository.List();
    }

    public Song? Single(Guid id)
    {
        return _repository.Single(id);
    }

    public void Save(Song song)
    {
        var validator = new SongValidator();
        var validationResult = validator.Validate(song);

        _repository.Save(song);
    }

    public void Delete(Guid id)
    {
        _repository.Delete(id);
    }
}
