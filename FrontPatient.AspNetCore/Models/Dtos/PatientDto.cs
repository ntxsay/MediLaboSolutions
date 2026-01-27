namespace FrontPatient.AspNetCore.Models.Dtos;

/// <summary>
/// DTO représentant un patient et utilisé suite aux appels API
/// </summary>
public record PatientDto
{
    public required int Id { get; init; }
    public required string FirstName { get; init; } = string.Empty;
    public required string LastName { get; init; } = string.Empty;
    public required DateOnly BirthDate { get; init; }
    public required string? PostalAddress { get; init; }
    public required string? NoTelephone { get; init; }
    public required int GenreId { get; init; }
    public required GenreDto Genre { get; init; } = null!;
    public required GenreDto[] Genres { get; init; } = [];
}