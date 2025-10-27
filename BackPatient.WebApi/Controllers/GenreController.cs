using BackPatient.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using PatientShared.Models.Dtos;

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
    public async Task<IActionResult> CreateGenreAsync([FromBody] GenreDto value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la création du genre ne sont pas valides.");
            return BadRequest();
        }
           
        var isCreated = await genreServices.CreateAsync(value);
        if (!isCreated)
            return BadRequest();

        var list = await genreServices.GetAllAsync();
        return Ok(list);
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
    public async Task<IActionResult> UpdateGenreAsync(int id, [FromBody] GenreDto value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la mise à jour du genre ne sont pas valides.");
            return BadRequest();
        }
           
        var isUpdated = await genreServices.UpdateAsync(id, value);
        if (!isUpdated)
            return BadRequest();

        var list = await genreServices.GetAllAsync();
        return Ok(list);
    }
}