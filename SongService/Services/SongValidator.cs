using FluentValidation;
using SongService.Entity;

namespace SongService.Services;

public class SongValidator : AbstractValidator<Song>
{
    public SongValidator(string prefixUri)
    {
        RuleFor(song => song.Title).NotEmpty();
        RuleFor(song => song.Artist).NotEmpty();
        RuleFor(song => song.ImageLink)
            .Must(url => url.StartsWith($"{prefixUri}/images"))
            .WithMessage("Image link must point to a trusted source.");
        RuleFor(song => song.AudioLink)
            .Must(url => url.StartsWith($"{prefixUri}/audio"))
            .WithMessage("Image link must point to a trusted source.");
    }
}