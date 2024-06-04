using FluentValidation.Results;
using SongService.Entity;
namespace SongService.Services;
public interface ISongService
{
    public Song[] List();

    public Song? Single(Guid id);

    public ValidationResult Save(Song song);

    public void Delete(Guid id);
}