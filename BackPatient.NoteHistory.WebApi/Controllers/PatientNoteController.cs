using BackPatient.NoteHistory.WebApi.Models.Dtos;
using BackPatient.NoteHistory.WebApi.Models.ViewModels;
using BackPatient.NoteHistory.WebApi.Services;
using BackPatient.NoteHistory.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BackPatient.NoteHistory.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientNoteController(IPatientNoteServices patientNoteServices, ILogger<PatientNoteController> logger) : ControllerBase
{

    /// <summary>
    /// Retourne toutes les notes de tous les patients confondus
    /// </summary>
    /// <returns></returns>
    [HttpGet("All")]
    public async Task<ActionResult<PatientNoteDto[]>> GetAllAsync() =>
        Ok(await patientNoteServices.GetAllAsync());
        
    /// <summary>
    /// Retourne toutes les notes d'un patient spécifique
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetByPatientId/{id:int}")]
    public async Task<ActionResult<PatientNoteDto[]>> GetAllByPatientIdAsync(int id) =>
        Ok(await patientNoteServices.GetAllByPatientIdAsync(id));
        
    /// <summary>
    /// Retourne toutes les notes d'un patient spécifique, contenant juste l'id du patient et la note
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("GetMinimalByPatientId/{id:int}")]
    public async Task<ActionResult<PatientNoteMinimalDto[]>> GetAllMinimalByPatientIdAsync(int id) =>
        Ok(await patientNoteServices.GetAllMinimalByPatientIdAsync(id));
    
    /// <summary>
    /// Retourne toutes les notes de plusieurs patients spécifiques, contenant juste l'id du patient et la note
    /// </summary>
    /// <param name="patientIds"></param>
    /// <returns></returns>
    [HttpGet("GetMinimalByPatientIds")]
    public async Task<ActionResult<PatientNoteMinimalDto[]>> GetAllMinimalByPatientIdsAsync([FromQuery] int[] patientIds) =>
        Ok(await patientNoteServices.GetAllMinimalByPatientIdsAsync(patientIds));

    /// <summary>
    /// Retourne une note spécifique
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>  
    [HttpGet("GetOne/{id:length(24)}")]
    public async Task<ActionResult<PatientNoteDto>> GetAsync(string id)
    {
        var data = await patientNoteServices.GetAsync(id);

        if (data is null)
        {
            return NotFound();
        }

        return Ok(data);
    }

    /// <summary>
    /// Insère une note dans la base de données
    /// </summary>
    /// <param name="formData"></param>
    /// <returns></returns>   
    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] PatientNoteViewModel formData)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la création de la note du patient ne sont pas valides.");
            return BadRequest();
        }
            
        var data = await patientNoteServices.CreateAsync(formData);
        if (data is null)
        {
            return BadRequest();
        }

        return Ok(data);
    }
        
    /// <summary>
    /// Insère plusieurs notes dans la base de données
    /// </summary>
    /// <param name="formDatas"></param>
    /// <returns></returns>
    [HttpPost("CreateRange")]
    public async Task<IActionResult> CreateRangeAsync([FromBody] PatientNoteViewModel[] formDatas)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la création de note patient ne sont pas valides.");
            return BadRequest();
        }
            
        var datas = await patientNoteServices.CreateRangeAsync(formDatas);
        if (datas.Length == 0)
        {
            return BadRequest();
        }

        return Ok(datas);
    }

    /// <summary>
    /// Met à jour une note dans la base de données
    /// </summary>
    /// <param name="id"></param>
    /// <param name="formData"></param>
    /// <returns></returns>
    [HttpPut("Update/{id:length(24)}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] PatientNoteViewModel formData)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la mise à jour de la note du patient \"{id}\" ne sont pas valides.", id);
            return BadRequest();
        }
            
        var data = await patientNoteServices.UpdateAsync(id, formData);
        if (data is null)
        {
            return BadRequest();
        }
            
        return Ok(data);
    }

    /// <summary>
    /// Supprime une note de la base de données
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("Delete/{id:length(24)}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var isDeleted = await patientNoteServices.RemoveAsync(id);

        if (!isDeleted)
        {
            return BadRequest();
        }

        return Ok(isDeleted);
    }
}