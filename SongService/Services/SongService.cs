using FluentValidation.Results;
using SongService.Entity;
using SongService.Repository;

namespace SongService.Services;

public class SongService(ISongRepository repository, EnvironmentVariableManager envManager) : ISongService
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

    public ValidationResult Save(Song song)
    {
        var validator = new SongValidator(prefixUri: envManager["API_GATEWAY_HOST"]);
        var validationResult = validator.Validate(song);

        if (validationResult.IsValid)
            _repository.Save(song);
        
        return validationResult;
    }

    public void Delete(Guid id)
    {
        _repository.Delete(id);
    }
}
