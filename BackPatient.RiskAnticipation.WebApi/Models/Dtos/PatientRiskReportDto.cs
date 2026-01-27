namespace BackPatient.RiskAnticipation.WebApi.Models.Dtos;

/// <summary>
/// Dto représentant un rapport de risque d'un patient
/// </summary>
public record PatientRiskReportDto
{
    public required int PatientId { get; init; }
    public required string Report { get; init; } = string.Empty;
}