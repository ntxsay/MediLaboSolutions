using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Models.ViewModels;
using BackPatient.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> CreatePatientAsync([FromBody] PatientViewModel value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la création du patient ne sont pas valides.");
            return BadRequest();
        }
        
        var data = await patientServices.CreateAsync(value);
        if (data == null)
        {
            logger.LogError("Le patient n'a pas été créé.");
            return BadRequest();
        }

        return Ok(data);
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
    
    [HttpGet("GetReportInfo/{id}")]
    public async Task<IActionResult> GetReportInfoAsync(int id)
    {
        var patient = await patientServices.GetReportInfoAsync(id);
        if (patient == null)
            return NotFound();
        return Ok(patient);
    }  
    
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> UpdatePatientAsync(int id, [FromBody] PatientViewModel value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la mise à jour du patient ne sont pas valides.");
            return BadRequest();
        }
           
        var data = await patientServices.UpdateAsync(id, value);
        if (data == null)
            return BadRequest();

        return Ok(data);
    }
}