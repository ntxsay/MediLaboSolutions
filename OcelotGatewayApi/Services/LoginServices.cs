using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OcelotGatewayApi.Models.Dtos;

namespace OcelotGatewayApi.Services;

public interface ILoginServices
{
    Task<TokenDto?> LoginAsync(LoginDto value);
}

public class LoginServices(ILogger<LoginServices> logger, UserManager<IdentityUser> userManager, IConfiguration configuration) : ILoginServices
{
    public async Task<TokenDto?> LoginAsync(LoginDto value)
    {
        try
        {
            if (string.IsNullOrEmpty(value.UserName) || string.IsNullOrWhiteSpace(value.UserName) ||
                string.IsNullOrEmpty(value.Password) || string.IsNullOrWhiteSpace(value.Password))
            {
                logger.LogError("Le nom d'utilisateur et le mot de passe sont obligatoires.");
                return null;
            }

            var user = await userManager.FindByNameAsync(value.UserName);
            if (user == null)
            {
                logger.LogWarning("L'utilisateur {UserName} n'a pas été trouvé.", value.UserName);
                return null;
            }
            if (!await userManager.CheckPasswordAsync(user, value.Password))
            {
                logger.LogWarning("La tentative de connexion a échoué pour l'utilisateur {UserName}.", value.UserName);
                return null;
            }

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (ClaimTypes.NameIdentifier, user.Id)
            };

            /*var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }*/

            var token = GetToken(authClaims);
            
            logger.LogInformation("Token généré pour l'utilisateur {UserName}.", value.UserName);
            
            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, "Une erreur est survenue lors de la connexion de l'utilisateur {UserName}.", value.UserName);
            return null;
        }
    }
    
    /// <summary>
    /// Crée et retourne un token JWT
    /// </summary>
    /// <param name="authClaims"></param>
    /// <returns></returns>
    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));
        var token = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }
}