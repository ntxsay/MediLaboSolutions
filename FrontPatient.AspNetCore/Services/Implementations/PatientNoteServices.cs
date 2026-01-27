using FrontPatient.AspNetCore.Models.Dtos;
using FrontPatient.AspNetCore.Models.ViewModels;
using FrontPatient.AspNetCore.Services.Interfaces;
using FrontPatient.AspNetCore.Utilities;

namespace FrontPatient.AspNetCore.Services.Implementations;

public class PatientNoteServices(ILogger<PatientNoteServices> logger, IHttpClientFactory clientFactory, IConfiguration configuration) : IPatientNoteServices, IDisposable
{
    private readonly HttpClient _client = clientFactory.CreateClient(configuration["MyHttpClients:GatewayClientName"]!);
    
    public async Task<PatientNoteViewModel[]> GetAllByPatientIdAsync(int id)
    {
        try
        {
            using var response = await _client.GetAsync($"PatientNote/GetByPatientId/{id}");
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la récupération des notes du patient n°{patientId} : {response}", id, response.ReasonPhrase);
                return [];
            }
            
            var datas = await response.Content.ReadFromJsonAsync<PatientNoteDto[]>();
            if (datas == null)
            {
                logger.LogWarning("Les notes du patient n°{patientId} n'ont pas été trouvés.", id);
                return [];  
            }

            return datas.Select(s => s.ToViewModel()).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, 
                "Une erreur est survenue lors de la récupération des notes du patient n°{id}", id);
            return [];
        }
    }
    
    public async Task<PatientNoteViewModel?> CreateAsync(PatientNoteViewModel value)
    {
        try
        {
            using var response = await _client.PostAsJsonAsync("PatientNote/Create", value.ToDto());
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la création de la note du patient n°{patientId} : {response}", value.PatientId, response.ReasonPhrase);
                return null;
            }
            
            var data = await response.Content.ReadFromJsonAsync<PatientNoteDto>();
            if (data == null)
            {
                logger.LogWarning("La note du patient n°{patientId} n'a pas été créée.", value.PatientId);
                return null;
            }

            return data.ToViewModel();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création de la note du patient n°{patientId}", value.PatientId);
            return null;
        }
    }
    
    public async Task<PatientNoteViewModel[]> CreateRangeAsync(PatientNoteViewModel[] values)
    {
        try
        {
            using var response = await _client.PostAsJsonAsync("PatientNote/CreateRange", values.Select(s => s.ToDto()).ToArray());
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Une erreur est survenue lors de la création de notes de patient : {response}", response.ReasonPhrase);
                return [];
            }
            
            var datas = await response.Content.ReadFromJsonAsync<PatientNoteDto[]>();
            if (datas == null)
            {
                logger.LogWarning("Les notes patients n'ont pas été créées.");
                return [];
            }

            return datas.Select(s => s.ToViewModel()).ToArray();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Une erreur est survenue lors de la création des notes patients.");
            return [];
        }
    }
    
    public void Dispose()
    {
        _client.Dispose();
    }
}