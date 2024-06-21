using FluentValidation.Results;
using SongService.Entity;

namespace SongService.Services;

public interface ISongService
{
    void Delete(Guid id);
    Song[] List();
    public void OnDeletedAudioOrImage(string uri);
    ValidationResult Save(Song song);
    Song? Single(Guid id);
}