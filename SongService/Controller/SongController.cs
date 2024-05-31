using Microsoft.AspNetCore.Mvc;
using SongService.Controller;
using SongService.Entity;
using SongService.Services;

namespace SongService.Controllers;

[ApiController]
[Route("songs")] 
public class SongController(ISongService songService) : ControllerBase
{
    private readonly ISongService _songService = songService;

    [HttpGet]
    public IActionResult List()
    {
        return Ok(_songService.List());
    }

    [HttpGet("{id}")]
    public IActionResult Single(Guid id)
    {
        var song = _songService.Single(id);

        return song is not null 
            ? Ok(song) 
            : NotFound(string.Empty);
    }

    [HttpPost]
    public IActionResult Create([FromBody] SongRequest request)
    {
        var validator = new SongRequestValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
            return BadRequest(result.Errors.Select(e => e.ErrorMessage));


        var song = new Song(request.Title!, request.Artist!, request.ImageLink!, request.AudioLink!);

        _songService.Save(song);

        return Ok();
    }

    [HttpPut("{id}")]
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

        _songService.Save(song);

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        _songService.Delete(id);

        return NoContent();
    }

}
