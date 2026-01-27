namespace FrontPatient.AspNetCore.Services.Interfaces;

/// <summary>
/// Interface de service permettant de gérer les rapports de risques de patients
/// </summary>
/// <remarks>Ce service effectue des appels vers l'API BackPatient.RiskAnticipation.WebApi via le Gateway</remarks>
public interface IPatientRiskReportServices
{
    /// <summary>
    /// Retourne le rapport de risques d'un patient
    /// </summary>
    /// <param name="patientId">Id du patient</param>
    /// <returns></returns>
    public Task<string?> GetPatientRiskReportAsync(int patientId);
}