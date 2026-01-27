namespace BackPatient.NoteHistory.WebApi.Models.Dtos;

/// <summary>
/// Dto représentant une note de patient minimal, juste l'id du patient et la note
/// </summary>
public record PatientNoteMinimalDto
{
    public required int PatientId { get; init; }
    public required string Note { get; init; } = string.Empty;
}