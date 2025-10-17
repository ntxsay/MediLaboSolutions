using BackPatient.WebApi.Datas;
using BackPatient.WebApi.Utilities;
using BackPatient.WebApi.Models.Dtos;
using BackPatient.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var datas = await patientServices.GetAllPatientsAsync();
        return Ok(datas);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreatePatientAsync([FromBody] PatientDto value)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Les données reçues pour la création du patient ne sont pas valides.");
            return BadRequest();
        }
           
        var isCreated = await patientServices.CreateAsync(value);
        if (!isCreated)
            return BadRequest();

        var list = await patientServices.GetAllPatientsAsync();
        return Ok(list);
    }
    
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetPatientAsync(int id)
    {
        var patient = await patientServices.GetAsync(id);
        if (patient == null)
            return NotFound();
        return Ok(patient);
    }
    
    [HttpPut("Update")]
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

        var list = await patientServices.GetAllPatientsAsync();
        return Ok(list);
    }
}