using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Models.ViewModels;

namespace FrontPatient.AspNetCore.Services.Interfaces;

public interface ILoginServices
{
    /// <summary>
    /// Authentifie un utilisateur et retourne un token JWT depuis la requête de connexion de l'utilisateur en provenance de l'API OcelotGatewayApi
    /// </summary>
    /// <param name="value">Le modèle de vue de connexion</param>
    /// <returns>Retourne un token JWT si l'authentification est reussie, sinon null.</returns>
    public Task<TokenDto?> LoginAsync(LoginViewModel value);
}