using Microsoft.AspNetCore.Mvc;
using OcelotGatewayApi.Models.Dtos;
using OcelotGatewayApi.Services;

namespace OcelotGatewayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(
        ILogger<AuthController> logger,
        ILoginServices loginServices)
        : ControllerBase
    {
        [HttpGet("Log")]
        public IActionResult GetHelloWorld()
        {
            return Ok("Hello world");
        }
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var token = await loginServices.LoginAsync(model);
            if (token == null)
            {
                return Unauthorized();
            }
            
            return Ok(token);
        }
    }
}