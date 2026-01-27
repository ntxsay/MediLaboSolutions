using OcelotGatewayApi.Models.Dtos;

namespace OcelotGatewayApi.Services.Interfaces;

/// <summary>
/// Interface de service permettant de gérer la connexion des utilisateurs.
/// </summary>
public interface ILoginServices
{
    /// <summary>
    /// Authentifie un utilisateur et retourne un token JWT.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Retourne un token JWT si l'authentification est reussie, sinon null.</returns>
    Task<TokenDto?> LoginAsync(LoginDto value);
}