namespace FrontPatient.AspNetCore.Models.Dtos;

/// <summary>
/// DTO représentant un genre et utilisé suite aux appels API 
/// </summary>
public record GenreDto
{
    public required int Id { get; init; }
    public required string Name { get; init; } = string.Empty;
    public required string? Description { get; init; }
}