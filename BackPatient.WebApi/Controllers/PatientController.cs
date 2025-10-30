using System.Text.Json;
using BackPatient.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using PatientShared.Models.Dtos;

namespace BackPatient.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientController(IPatientServices patientServices, ILogger<PatientController> logger) : ControllerBase
{
    [HttpGet("All")]
    [ProducesResponseType(typeof(PatientDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllPatientsAsync()
    {
        var datas = await patientServices.GetAllAsync();
        return Ok(datas);
    }
    
    [HttpGet("CreateEmpty")]
    public async Task<IActionResult> CreateEmptyPatientAsync()
    {
        var data = await patientServices.CreateEmptyAsync();
        return Ok(data);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreatePatientAsync([FromBody] PatientDto value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la création du patient ne sont pas valides.");
            return BadRequest();
        }
        
        var json = JsonSerializer.Serialize(value);
        logger.LogInformation(json);   
        
        var isCreated = await patientServices.CreateAsync(value);
        if (!isCreated)
        {
            logger.LogError("Le patient n'a pas été créé.");
            return BadRequest();
        }

        var list = await patientServices.GetAllAsync();
        return Ok(list);
    }
    
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> DetailsPatientAsync(int id)
    {
        var patient = await patientServices.DetailsAsync(id);
        if (patient == null)
            return NotFound();
        return Ok(patient);
    }
    
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetPatientAsync(int id)
    {
        var patient = await patientServices.GetAsync(id);
        if (patient == null)
            return NotFound();
        return Ok(patient);
    }
    
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdatePatientAsync(int id, [FromBody] PatientDto value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la mise à jour du patient ne sont pas valides.");
            return BadRequest();
        }
           
        var isUpdated = await patientServices.UpdateAsync(id, value);
        if (!isUpdated)
            return BadRequest();

        var list = await patientServices.GetAllAsync();
        return Ok(list);
    }
}