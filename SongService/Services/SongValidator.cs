using FluentValidation;
using SongService.Entity;

namespace SongService.Services;
public class SongValidator : AbstractValidator<Song>
{
    public SongValidator()
    {
        RuleFor(song => song.Title).NotEmpty();
        RuleFor(song => song.Artist).NotEmpty();
        RuleFor(song => song.ImageLink)
            .Must(url => url.StartsWith("https://api.respotify.org/images"))
            .WithMessage("Image link must point to a trusted source.");
        RuleFor(song => song.AudioLink)
            .Must(url => url.StartsWith("https://api.respotify.org/audio"))
            .WithMessage("Image link must point to a trusted source.");
    }
}