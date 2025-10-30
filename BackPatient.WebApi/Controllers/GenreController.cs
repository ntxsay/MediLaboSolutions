using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Models.ViewModels;
using BackPatient.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackPatient.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenreController(IGenreServices genreServices, ILogger<GenreController> logger) : ControllerBase
{
    [HttpGet("All")]
    [ProducesResponseType(typeof(GenreDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllGenresAsync()
    {
        var datas = await genreServices.GetAllAsync();
        return Ok(datas);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateGenreAsync([FromBody] GenreViewModel value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la création du genre ne sont pas valides.");
            return BadRequest();
        }
           
        var data = await genreServices.CreateAsync(value);
        if (data == null)
            return BadRequest();

        return Ok(data);
    }
    
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetGenreAsync(int id)
    {
        var genre = await genreServices.GetAsync(id);
        if (genre == null)
            return NotFound();
        return Ok(genre);
    }
    
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdateGenreAsync(int id, [FromBody] GenreViewModel value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la mise à jour du genre ne sont pas valides.");
            return BadRequest();
        }
           
        var data = await genreServices.UpdateAsync(id, value);
        if (data == null)
            return BadRequest();

        return Ok(data);
    }
}