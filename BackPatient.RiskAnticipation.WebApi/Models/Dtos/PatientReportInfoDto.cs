namespace BackPatient.RiskAnticipation.WebApi.Models.Dtos;

/// <summary>
/// Dto contenant les informations d'un patient pour la génération d'un rapport de risque
/// </summary>
public record PatientReportInfoDto
{
    public required int PatientId { get; init; }
    public required byte PatientAge { get; init; }
    public required string PatientGender { get; init; } = string.Empty;
}