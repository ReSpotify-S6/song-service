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
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            .WithMessage("Image link must be a valid HTTP or HTTPS link.");
        RuleFor(song => song.AudioLink)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            .WithMessage("Audio link must be a valid HTTP or HTTPS link.");
    }
}