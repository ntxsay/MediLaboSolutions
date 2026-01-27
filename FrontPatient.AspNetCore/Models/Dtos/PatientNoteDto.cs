namespace FrontPatient.AspNetCore.Models.Dtos;

/// <summary>
/// DTO représentant une note d'un patient et utilisé suite aux appels API
/// </summary>
public record PatientNoteDto
{
    /// <summary>
    /// Id (MongoDB) de la note
    /// </summary>
    public required string? Id { get; init; }
    public required int PatientId { get; init; }
    public required string PatientName { get; init; } = string.Empty;
    public required string Note { get; init; } = string.Empty;
}