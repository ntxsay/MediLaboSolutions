namespace OcelotGatewayApi.Models.Dtos;

/// <summary>
/// Dto permettant de sérialiser le token et sa date d'expiration.
/// </summary>
public record TokenDto
{
    public required string? Token { get; init; }
    public required DateTime? Expiration { get; init; }
}