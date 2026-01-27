namespace OcelotGatewayApi.Models.Dtos;

/// <summary>
/// Dto permettant de stocker le login et le mot de passe du formulaire de connexion.
/// </summary>
public record LoginDto
{
    public required string UserName { get; init; }
    public required string Password { get; init; }
}