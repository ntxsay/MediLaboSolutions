namespace FrontPatient.AspNetCore.Models.Dtos;

/// <summary>
/// DTO représentant un rapport de risque d'un patient et utilisé suite aux appels API
/// </summary>
public record PatientRiskReportDto
{
    public required int PatientId { get; init; }
    public required string Report { get; init; } = string.Empty;
}