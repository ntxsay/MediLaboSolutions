namespace BackPatient.RiskAnticipation.WebApi.Models.Dtos;

/// <summary>
/// Dto contenant juste l'id du patient et la note
/// </summary>
public record PatientNoteMinimalDto
{
    public required int PatientId { get; init; }
    public required string Note { get; init; } = string.Empty;
}