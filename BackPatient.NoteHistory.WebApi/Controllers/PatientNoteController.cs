using BackPatient.NoteHistory.WebApi.Models.Dtos;
using BackPatient.NoteHistory.WebApi.Models.ViewModels;
using BackPatient.NoteHistory.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackPatient.NoteHistory.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientNoteController(IPatientNoteServices patientNoteServices, ILogger<PatientNoteController> logger) : ControllerBase
    {

        [HttpGet("All")]
        public async Task<ActionResult<PatientNoteDto[]>> GetAllAsync() =>
            Ok(await patientNoteServices.GetAllAsync());
        
        [HttpGet("GetByPatientId/{id:int}")]
        public async Task<ActionResult<PatientNoteDto[]>> GetAllByPatientIdAsync(int id) =>
            Ok(await patientNoteServices.GetAllByPatientIdAsync(id));

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
}
