using BackPatient.RiskAnticipation.WebApi.Models.Dtos;

namespace BackPatient.RiskAnticipation.WebApi.Services.Implementations;

public class RiskAnticipationServices(ILogger<RiskAnticipationServices> logger, IHttpClientFactory clientFactory, IConfiguration configuration) : IRiskAnticipationServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient(configuration["MyHttpClients:GatewayClientName"]!);
    
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
    
    private string GenerateRiskReport(byte patientAge, string patientGender, string[] patientNotes)
    {
        // Calcul des occurrences de triggers
        int countTriggers = patientNotes
            .Sum(note => _terminologies.Count(t => 
                note.Contains(t, StringComparison.OrdinalIgnoreCase)));

        logger.LogInformation("count triggers : {count}", countTriggers);

        if (countTriggers == 0)
            return "Aucun risque";

        // -----------------------------
        //   Moins de 30 ans
        // -----------------------------
        if (patientAge < 30)
        {
            bool isFemale = patientGender.Equals("F", StringComparison.OrdinalIgnoreCase);
            bool isMale   = patientGender.Equals("M", StringComparison.OrdinalIgnoreCase);

            if (isFemale)
            {
                if (countTriggers == 4) return "Danger";
                if (countTriggers >= 7) return "Apparition précoce";
            }
            else if (isMale)
            {
                if (countTriggers == 3) return "Danger";
                if (countTriggers >= 5) return "Apparition précoce";
            }

            return "Aucun risque";
        }

        // -----------------------------
        //   Plus de 30 ans
        // -----------------------------
        if (patientAge > 30)
        {
            return countTriggers switch
            {
                >= 2 and <= 5 => "Risque limité",
                6 or 7        => "Danger",
                >= 8          => "Apparition précoce",
                _             => "Aucun risque"
            };
        }

        // Cas âge == 30 : non défini → Aucun risque
        return "Aucun risque";
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