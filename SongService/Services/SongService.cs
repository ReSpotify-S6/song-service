using FluentValidation.Results;
using SongService.Entity;
using SongService.Repository;

namespace SongService.Services;

public class SongService(SongContext context, IReadOnlyDictionary<string, string> envStore, ILogger<SongService>? logger = null) : ISongService
{
    public Song[] List()
    {
        return context.Songs.ToArray();
    }

    public Song? Single(Guid id)
    {
        return context.Songs.Find(id);
    }

    public ValidationResult Save(Song song)
    {
        var validator = new SongValidator(prefixUri: envStore["API_GATEWAY_HOST"]);
        var validationResult = validator.Validate(song);

        if (validationResult.IsValid)
        {
            context.Add(song);
            context.SaveChanges();
        }

        return validationResult;
    }

    public void Delete(Guid id)
    {
        var song = context.Songs.Find(id);
        if (song != null)
        {
            context.Songs.Remove(song);
            context.SaveChanges();
        }
    }

    public void OnDeletedAudioOrImage(string uri)
    {
        var songsToRemove = context.Songs.Where(s => s.AudioLink == uri || s.ImageLink == uri);
        if (logger is not null)
        {
            logger.LogInformation("Resource deleted: {}", uri);
            logger.LogInformation("Removing song metadatas: {}", songsToRemove.Select(s => $"{s.Title} - {s.Artist}"));
        }
        context.Songs.RemoveRange(songsToRemove);
        context.SaveChanges();
    }
}
