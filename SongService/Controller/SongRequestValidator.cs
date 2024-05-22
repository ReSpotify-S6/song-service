using FluentValidation;
using SongService.Controllers;

namespace SongService.Controller;

public class SongRequestValidator : AbstractValidator<SongRequest>
{
    public SongRequestValidator()
    {
        RuleFor(request => request.Title).NotNull();
        RuleFor(request => request.Artist).NotNull();
        RuleFor(request => request.ImageLink).NotNull();
        RuleFor(request => request.AudioLink).NotNull();
    }
}