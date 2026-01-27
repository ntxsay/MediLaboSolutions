using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Services.Interfaces;

namespace FrontPatient.AspNetCore.Services.Implementations;

/// <summary>
/// Service permettant de gérer les rapports de risques de patients
/// </summary>
/// <remarks>Ce service effectue des appels vers l'API BackPatient.RiskAnticipation.WebApi via le Gateway</remarks>
public sealed class PatientRiskReportServices(ILogger<PatientRiskReportServices> logger, IHttpClientFactory clientFactory, IConfiguration configuration) : IPatientRiskReportServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient(configuration["MyHttpClients:GatewayClientName"]!);

    public async Task<string?> GetPatientRiskReportAsync(int patientId)
    {
        try
        {
            using var response = await _client.GetAsync($"PatientRiskAnticipation/GetRiskReport/{patientId}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération du rapport du patient n°{patientId} : {response}", patientId, response.ReasonPhrase);
                return null;
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientRiskReportDto>();
            if (data == null)
            {
                logger.LogWarning("Le rapport du patient n°{patientId} n'a pas été trouvé.", patientId);
                return null;
            }

            return data.Report;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, 
                "Une erreur est survenue lors de la récupération du rapport du patient n°{id}", patientId);
            return null;
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
    
}