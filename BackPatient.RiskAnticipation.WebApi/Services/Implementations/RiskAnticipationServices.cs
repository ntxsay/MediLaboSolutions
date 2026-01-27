using BackPatient.RiskAnticipation.WebApi.Models.Dtos;
using BackPatient.RiskAnticipation.WebApi.Services.Interfaces;

namespace BackPatient.RiskAnticipation.WebApi.Services.Implementations;

/// <summary>
/// Service permettant de générer un rapport de risque pour un patient
/// </summary>
public sealed class RiskAnticipationServices(ILogger<RiskAnticipationServices> logger, IHttpClientFactory clientFactory, IConfiguration configuration) : IRiskAnticipationServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient(configuration["MyHttpClients:GatewayClientName"]!);
    
    /// <summary>
    /// Récupère les informations d'un patient pour l'évaluation du risque
    /// </summary>
    /// <param name="patientId">L'id du patient</param>
    /// <returns>Si les informations sont trouvées, retourne un PatientReportInfoDto, sinon null</returns>
    private async Task<PatientReportInfoDto?> GetPatientRiskInfoAsync(int patientId)
    {
        try
        {
            using var response = await _client.GetAsync($"patient/GetReportInfo/{patientId}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération des informations nécessaires à l'évaluation du risque du patient : {response}", response.ReasonPhrase);
                return null;
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientReportInfoDto>();
            if (data == null)
            {
                logger.LogWarning("Les informations nécessaires à l'évaluation du risque du patient n'ont pas été trouvés.");
                return null;
            }

            return data;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération des informations nécessaires à l'évaluation du risque du patient");
            return null;
        }
    }
    
    /// <summary>
    /// Récupère les notes d'un patient pour l'évaluation du risque
    /// </summary>
    /// <param name="patientId">L'id du patient</param>
    /// <returns>Si les notes sont trouvées, retourne un tableau de string, sinon un tableau vide</returns>
    private async Task<string[]> GetPatientNoteAsync(int patientId)
    {
        try
        {
            using var response = await _client.GetAsync($"PatientNote/GetMinimalByPatientId/{patientId}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération des notes nécessaire à l'évaluation du risque du patient : {response}", response.ReasonPhrase);
                return [];
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientNoteMinimalDto[]>();
            if (data == null || data.Length == 0)
            {
                logger.LogWarning("Les notes nécessaires à l'évaluation du risque du patient n'ont pas été trouvés.");
                return [];
            }

            return data.Select(s => s.Note).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la récupération des notes nécessaire à l'évaluation du risque du patient");
            return [];
        }
    }
    
    public async Task<PatientRiskReportDto?> GeneratePatientRiskReport(int patientId)
    {
        var patientReportInfo = await GetPatientRiskInfoAsync(patientId);
        if (patientReportInfo == null)
            return null;
        
        var patientNotes = await GetPatientNoteAsync(patientId);
        if (patientNotes.Length == 0)
            return null;

        return new PatientRiskReportDto
        {
            PatientId = patientReportInfo.PatientId,
            Report = GenerateRiskReport(patientReportInfo.PatientAge, patientReportInfo.PatientGender, patientNotes)
        };
    }
    
    private string GenerateRiskReport(byte patientAge, string patientGender, string[] patientNotes)
    {
        // Calcule la somme du nombre de terminologies trouvées par note
        var numberOfTerminologyFounded = patientNotes
            .Sum(note => Terminologies.Count(t => 
                note.Contains(t, StringComparison.OrdinalIgnoreCase)));

        logger.LogInformation("Nombre total de terminologies trouvées : {count}", numberOfTerminologyFounded);

        if (numberOfTerminologyFounded == 0)
            return "Aucun risque";

        // calcul pour les moins de 30 ans
        if (patientAge < 30)
        {
            var isFemale = patientGender.Equals("F", StringComparison.OrdinalIgnoreCase);
            var isMale   = patientGender.Equals("M", StringComparison.OrdinalIgnoreCase);

            if (isFemale)
            {
                if (numberOfTerminologyFounded == 4) 
                    return "Danger";
                if (numberOfTerminologyFounded >= 7) 
                    return "Apparition précoce";
            }
            else if (isMale)
            {
                if (numberOfTerminologyFounded == 3) return "Danger";
                if (numberOfTerminologyFounded >= 5) return "Apparition précoce";
            }

            return "Aucun risque";
        }

        // calcul pour les plus de 30 ans
        if (patientAge > 30)
        {
            return numberOfTerminologyFounded switch
            {
                >= 2 and <= 5 => "Risque limité",
                6 or 7        => "Danger",
                >= 8          => "Apparition précoce",
                _             => "Aucun risque"
            };
        }

        // pour les personnes ayant 30 ans, le cas n'est pas défini donc du coup on retourne "Aucun risque"
        return "Aucun risque";
    }


    private static readonly string[] Terminologies =
    [
        "Hémoglobine A1C",
        "Microalbumine",
        "Taille",
        "Poids",
        "Fumeur",
        "Fumeuse",
        "Anormal",
        "Cholestérol",
        "Vertiges",
        "Rechute",
        "Réaction",
        "Anticorps"
    ];
    
    public void Dispose()
    {
        _client.Dispose();
    }
}