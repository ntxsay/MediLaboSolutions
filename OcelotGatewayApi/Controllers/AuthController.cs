using Microsoft.AspNetCore.Mvc;
using OcelotGatewayApi.Models.Dtos;
using OcelotGatewayApi.Services.Interfaces;

namespace OcelotGatewayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ILoginServices loginServices)
        : ControllerBase
    {
        /// <summary>
        /// Authentifie un utilisateur et retourne un token JWT.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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