using BackPatient.RiskAnticipation.WebApi.Models.Dtos;

namespace BackPatient.RiskAnticipation.WebApi.Services;

public interface IRiskAnticipationServices
{
    public Task<PatientRiskReportDto?> GeneratePatientRiskReport(int patientId);
}

public class RiskAnticipationServices(ILogger<RiskAnticipationServices> logger, IHttpClientFactory clientFactory) : IRiskAnticipationServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient("GatewayClient");
    
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
    
    private static string GenerateRiskReport(byte patientAge, string patientGender, string[] patientNotes)
    {
        var countTriggers = patientNotes.Count(a => _terminologies.Contains(a, StringComparer.OrdinalIgnoreCase));
        if (countTriggers == 0)
            return "Aucun risque";
        
        if (patientAge < 30)
        {
            switch (patientGender.ToUpper())
            {
                case "F":
                {
                    switch (countTriggers)
                    {
                        case 4:
                            return "Danger";
                        case >= 7:
                            return "Apparition précoce";
                    }
                    break;
                }
                case "M":
                {
                    switch (countTriggers)
                    {
                        case 3:
                            return "Danger";
                        case >= 5:
                            return "Apparition précoce";
                    }

                    break;
                }
                default:
                    break;
            }
        }
        else if (patientAge > 30)
        {
            switch (countTriggers)
            {
                case >=2 and <= 5:
                    return "Risque limité";
                case 6 or 7:
                    return "Danger";
                case >= 8:
                    return "Apparition précoce";
            }
        }

        return "Risque non évalué";
    }

    private static readonly string[] _terminologies =
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