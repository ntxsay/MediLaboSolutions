using BackPatient.RiskAnticipation.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackPatient.RiskAnticipation.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientRiskAnticipationController(ILogger<PatientRiskAnticipationController> logger, IRiskAnticipationServices riskAnticipationServices) : ControllerBase
    {

        [HttpGet("GetRiskReport/{id:int}")]
        public async Task<IActionResult> GeneratePatientRiskReport(int id)
        {
            var data = await riskAnticipationServices.GeneratePatientRiskReport(id);
            if (data == null)
                return BadRequest();
            
            return Ok(data);
        }
    }
}
