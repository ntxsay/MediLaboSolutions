namespace BackPatient.NoteHistory.WebApi.Models.Dtos;

/// <summary>
/// Dto représentant une note de patient
/// </summary>
public record PatientNoteDto
{
    public required string? Id { get; init; }
    public required int PatientId { get; init; }
    public required string PatientName { get; init; } = string.Empty;
    public required string Note { get; init; } = string.Empty;
}