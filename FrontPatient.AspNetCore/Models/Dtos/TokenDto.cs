namespace FrontPatient.AspNetCore.Models.Dtos;

/// <summary>
/// Dto permettant de retourner le token et sa date d'expiration depuis la requête de connexion de l'utilisateur
/// </summary>
public record TokenDto
{
    public required string? Token { get; init; }
    public required DateTime? Expiration { get; init; }
}