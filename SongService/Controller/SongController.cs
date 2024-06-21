using Microsoft.AspNetCore.Mvc;
using SongService.Authorization;
using SongService.Entity;
using SongService.Services;

namespace SongService.Controller;

[ApiController]
[Route("songs")]
public class SongController(ISongService songService) : ControllerBase
{

    [HttpGet]
    [Allow("user")]
    public IActionResult List()
    {
        return Ok(songService.List());
    }

    [HttpGet("{id}")]
    [Allow("user")]
    public IActionResult Single(Guid id)
    {
        var song = songService.Single(id);

        return song is not null
            ? Ok(song)
            : NotFound(string.Empty);
    }

    [HttpPost]
    [Allow("administrator")]
    public IActionResult Create([FromBody] SongRequest request)
    {
        var validator = new SongRequestValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));


        var song = new Song(request.Title!, request.Artist!, request.ImageLink!, request.AudioLink!);

        var validationResult = songService.Save(song);



        if (validationResult.IsValid)
            return Ok();
        else
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
    }

    [HttpPut("{id}")]
    [Allow("administrator")]
    public IActionResult Update(Guid id, [FromBody] SongRequest request)
    {
        var validator = new SongRequestValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));

        var song = new Song(request.Title!, request.Artist!, request.ImageLink!, request.AudioLink!)
        {
            Id = id
        };

        var validationResult = songService.Save(song);

        if (validationResult.IsValid)
            return Ok();
        else
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
    }

    [HttpDelete("{id}")]
    [Allow("administrator")]
    public IActionResult Delete(Guid id)
    {
        songService.Delete(id);

        return NoContent();
    }

}
