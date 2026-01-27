using BackPatient.RiskAnticipation.WebApi.Models.Dtos;

namespace BackPatient.RiskAnticipation.WebApi.Services.Interfaces;

/// <summary>
/// Interface de service permettant de gérer les rapports de risque des patients
/// </summary>
public interface IRiskAnticipationServices
{
    /// <summary>
    /// Génère un rapport de risque pour un patient
    /// </summary>
    /// <param name="patientId">Id du patient</param>
    /// <returns></returns>
    public Task<PatientRiskReportDto?> GeneratePatientRiskReport(int patientId);
}